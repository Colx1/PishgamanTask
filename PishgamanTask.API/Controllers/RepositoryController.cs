using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
	[Authorize(Roles = "Admin")]
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
		[Route("GetPerson/{id}")]
		public async Task<ActionResult<PersonDTO>> GetPerson([FromRoute] int id)
		{
			_logger.LogInformation($"GetPerson - ID: {id}");

			var results = await _personService.GetPersonAsync(id);
			return Ok(results);
		}
		#endregion

		#region Methods: POST
		[HttpPost]
		[Route("InsertPerson")]
		public async Task<ActionResult<PersonDTO>> InsertPerson([FromBody] PersonDTO person)
		{
			_logger.LogInformation($"InsertPerson - {JsonSerializer.Serialize(person)}");

			var personMapped = _mapper.Map<Person>(person);

			if (await _personService.IsPhoneNumberExistsOnInsert(personMapped.PhoneNumber))
				return BadRequest("PhoneNumber already exists in database!");

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

			if (await _personService.IsPhoneNumberExistsOnUpdate(personMapped))
				return BadRequest("PhoneNumber already exists in database, Can't update that!");

			var results = await _personService.UpdatePersonAsync(personMapped);
			return Ok(results);
		}
		#endregion

		#region Methods: DELETE
		[HttpDelete]
		[Route("DeletePerson/{id}")]
		public async Task<ActionResult<bool>> DeletePerson([FromRoute] int id)
		{
			_logger.LogInformation($"DeletePerson - ID: {id}");

			var results = await _personService.DeletePersonAsync(id);
			return Ok(results);
		}
		#endregion
	}
}
