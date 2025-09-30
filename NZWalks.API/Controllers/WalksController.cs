using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository) 
        { 
            _mapper = mapper;
            _walkRepository = walkRepository;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            

            var walkModel = _mapper.Map<Walk>(addWalkRequestDto);
            walkModel = await _walkRepository.CreateAsync(walkModel);

            return Ok(_mapper.Map<WalkDto>(walkModel));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn,
                                                [FromQuery] string? filterQuery,
                                                [FromQuery] string? sortBy,
                                                [FromQuery] bool? isAscending,
                                                [FromQuery] int pageNumber = 1,
                                                [FromQuery] int pageSize = 200)
        {
            var walksModel = await _walkRepository.GetAllAsync(filterOn, filterQuery,
                                                               sortBy, isAscending ?? true,
                                                               pageNumber, pageSize);

            var walksDto =  _mapper.Map<List<WalkDto>>(walksModel);
            return Ok(walksDto);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkModel = await _walkRepository.GetByIdAsync(id);

            if (walkModel == null) return NotFound();

            return Ok(_mapper.Map<WalkDto>(walkModel));
        }

        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            var walkModel = _mapper.Map<Walk>(updateWalkRequestDto);

            walkModel = await _walkRepository.UpdateAsync(id, walkModel);
            if (walkModel == null) return NotFound();

            return Ok(_mapper.Map<WalkDto>(walkModel));
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkModel = await _walkRepository.DeleteAsync(id);

            if (walkModel == null) return NotFound();

            return Ok(_mapper.Map<WalkDto>(walkModel));
        }
    }
}
