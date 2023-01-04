using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Data;
using CommandAPI.Models;
using AutoMapper;
using CommandAPI.Dtos;
using Microsoft.AspNetCore.JsonPatch;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandAPIRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandAPIRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // [HttpGet]
        // public ActionResult<IEnumerable<string>> Get()
        // {
        //     return new string[] {"this", "is", "hard", "coded"};
        // }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = _repository.GetAllCommands();
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        [HttpGet("{id}", Name="GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = _repository.GetCommandById(id);

            if(commandItem == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CommandReadDto>(commandItem));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand
            (CommandCreateDto commandCreateDto)
        {
            //create a CreateDto and save to Db
            var commandModel = _mapper.Map<Command>(commandCreateDto);
            _repository.CreateCommand(commandModel);
            _repository.SaveChanges();

            //create a ReadDto to return as output for this method
            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            return CreatedAtRoute(nameof(GetCommandById),
                new {Id = commandReadDto.Id}, commandReadDto);
        }

        [HttpPut("{id}")]
        public ActionResult<CommandUpdateDto> UpdateCommand
            (int id, CommandUpdateDto commandUpdateDto)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if(commandModelFromRepo == null)
            {
                return NotFound();
            }            
            _mapper.Map(commandUpdateDto, commandModelFromRepo);
            
            //this currently calls an 'empty' method. Put here in case in future the repository's 
            //implementation of UpdateCommand will be changed so we don't need to update the controller.
            _repository.UpdateCommand(commandModelFromRepo);            
            _repository.SaveChanges();

            return NoContent();            
        }

        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpate(int id,
            JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            //get command model from repo
            var commandModelFromRepo = _repository.GetCommandById(id);
            if(commandModelFromRepo == null)
            {
                return NotFound();
            }
            //map model from repo to DTO fior the JsonPatchDocument to apply to the DTO object
            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);
            //apply the provided PatchDocument to the DTO to be updated
            patchDoc.ApplyTo(commandToPatch, ModelState);
            //check if the updates are valid by checking the modelstate
            if(!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }
            //apply the mapping to the model from the repo
            _mapper.Map(commandToPatch, commandModelFromRepo);
            //update repo and save changes
            _repository.UpdateCommand(commandModelFromRepo);
            _repository.SaveChanges();
            //noothing to return for the patch request
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if(commandModelFromRepo == null)
            {
                return NotFound();
            }

            _repository.DeleteCommand(commandModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }

    }
}