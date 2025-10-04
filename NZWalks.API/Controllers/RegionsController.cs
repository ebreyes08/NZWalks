using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(NZWalksDbContext dbContext,
                                 IRegionRepository regionRepository,
                                 IMapper mapper)
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regions = await _regionRepository.GetAllAsync();

            var regionsDto = _mapper.Map<List<RegionDto>>(regions);

            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region = await _regionRepository.GetByIdAsync(id);

            if (region == null) return NotFound();

            var regionDto = _mapper.Map<RegionDto>(region);

            return Ok(regionDto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            var regionModel = _mapper.Map<Region>(addRegionRequestDto);

            regionModel = await _regionRepository.CreateAsync(regionModel);

            var regionDto = _mapper.Map<RegionDto>(regionModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);

        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionModel = _mapper.Map<Region>(updateRegionRequestDto);

            regionModel = await _regionRepository.UpdateAsync(id, regionModel);

            if (regionModel == null) return NotFound();

            var regionDto = _mapper.Map<RegionDto>(regionModel);

            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionModel = await _regionRepository.DeleteAsync(id);

            if (regionModel == null) return NotFound();

            var regionDto = _mapper.Map<RegionDto>(regionModel);

            return Ok(regionDto);
        }
    }
}
