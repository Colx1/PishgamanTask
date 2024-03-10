using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PishgamanTask.API.DTOs;
using PishgamanTask.Application.Interfaces;
using PishgamanTask.Domain.Entities;
using System;
using System.Text.Json;

namespace PishgamanTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepositoryController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly ILogger<RepositoryController> _logger;
        private readonly IMapper _mapper;

        public RepositoryController(IPersonService personService, ILogger<RepositoryController> logger, IMapper mapper)
        {
            _personService = personService;
            _logger = logger;
            _mapper = mapper;

            _logger.LogInformation($"RepositoryController ctor made!");
        }

        #region Methods: GET
        [HttpGet]
        [Route("GetAllPersons")]
        public async Task<ActionResult<IEnumerable<PersonDTO>>> GetAllPersons()
        {
            _logger.LogInformation($"GetAllPersons");

            var results = await _personService.GetAllPersonsAsync();
            return Ok(results);
        }

        [HttpGet]
        [Route("GetPerson")]
        public async Task<ActionResult<PersonDTO>> GetPerson([FromBody]int id)
        {
            _logger.LogInformation($"GetPerson - ID: {id}");

            var results = await _personService.GetPersonAsync(id);
            return Ok(results);
        }
        #endregion

        #region Methods: POST
        [HttpPost]
        [Route("InsertPerson")]
        public async Task<ActionResult<PersonDTO>> InsertPerson([FromBody]PersonDTO person)
        {
            _logger.LogInformation($"InsertPerson - {JsonSerializer.Serialize(person)}");

            var personMapped = _mapper.Map<Person>(person);
            var results = await _personService.InsertNewPersonAsync(personMapped);
            return Ok(results);
        }
        #endregion

        #region Methods: PUT
        [HttpPut]
        [Route("UpdatePerson")]
        public async Task<ActionResult<PersonDTO>> UpdatePerson([FromBody] PersonDTO person)
        {
            _logger.LogInformation($"UpdatePerson - {JsonSerializer.Serialize(person)}");

            var personMapped = _mapper.Map<Person>(person);
            var results = await _personService.UpdatePersonAsync(personMapped);
            return Ok(results);
        }
        #endregion

        #region Methods: DELETE
        [HttpDelete]
        [Route("DeletePerson")]
        public async Task<ActionResult<bool>> DeletePerson([FromBody]int id)
        {
            _logger.LogInformation($"DeletePerson - ID: {id}");

            var results = await _personService.DeletePersonAsync(id);
            return Ok(results);
        }
        #endregion
    }
}
