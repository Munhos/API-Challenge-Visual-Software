using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services.FlightsController;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MonitoredFlightController : ControllerBase
    {
        private readonly FlightsService _flightService;

        public MonitoredFlightController(FlightsService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet("getAllExternal")]
        [SwaggerOperation(Summary = "Obtém todos os voos monitorados externos")]
        [SwaggerResponse(200, "Lista de voos monitorados", typeof(List<MonitoredFlightsModel>))]
        [SwaggerResponse(404, "Nenhum voo encontrado")]
        public async Task<List<MonitoredFlightsModel>> ShowAllFlights()
        {
            try
            {
                var flights = await _flightService.GetAllFlightsExternalApiData();

                if (flights == null || !flights.Any())
                {
                    return new List<MonitoredFlightsModel>();
                }

                return flights.ToList();
            }
            catch
            {
                Response.StatusCode = 500;
                return new List<MonitoredFlightsModel>();
            }
        }

        [HttpGet("getOne/{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Obtém um voo monitorado pelo ID")]
        [SwaggerResponse(200, "Voo monitorado encontrado", typeof(MonitoredFlightsModel))]
        [SwaggerResponse(400, "ID inválido")]
        public async Task<ActionResult> GetOneFlightsExternalApiData(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest("ID inválido");
                }

                var result = await _flightService.GetOneFlightsExternalApiData(id);
                return Ok(result);
            }
            catch
            {
                return StatusCode(500, "Erro ao buscar voo monitorado.");
            }
        }

        [HttpGet("getAllinternal")]
        [Authorize]
        [SwaggerOperation(Summary = "Obtém todos os dados de voos internos monitorados")]
        [SwaggerResponse(200, "Lista de voos internos monitorados", typeof(IEnumerable<MonitoredFlightsModel>))]
        public async Task<ActionResult> GetAllFlightInternalData()
        {
            try
            {
                var result = await _flightService.GetAllFlightInternalData();
                return Ok(result);
            }
            catch
            {
                return StatusCode(500, "Erro ao buscar voos internos monitorados.");
            }
        }

        [HttpPost("postInternal")]
        [Authorize]
        [SwaggerOperation(Summary = "Cria um novo voo monitorado interno")]
        [SwaggerResponse(201, "Voo monitorado criado com sucesso", typeof(MonitoredFlightsModel))]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<ActionResult> CreateRegisterMonitoredFlyght([FromBody] FlyghtLinkedInUserModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Model inválido");
                }

                var result = await _flightService.CreateRegisterMonitoredFlyght(model);
                return Ok(result);
            }
            catch
            {
                return StatusCode(500, "Erro ao criar voo monitorado.");
            }
        }

        [HttpDelete("deleteInternal/{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Deleta um voo monitorado interno pelo ID")]
        [SwaggerResponse(200, "Voo deletado com sucesso")]
        [SwaggerResponse(400, "ID inválido")]
        public async Task<ActionResult> DeleteRegisterLinkedInUserFlyghts(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest("Id inválido");
                }

                var result = await _flightService.DeleteRegisterLinkedInUserFlyghts(id);
                return Ok(result);
            }
            catch
            {
                return StatusCode(500, "Erro ao deletar voo monitorado.");
            }
        }

        [HttpPatch("patchInternal/{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Atualiza um voo monitorado interno")]
        [SwaggerResponse(200, "Voo atualizado com sucesso", typeof(MonitoredFlightsModel))]
        [SwaggerResponse(400, "ID ou dados inválidos")]
        public async Task<ActionResult> PatchLinkedInUserFlyghts([FromBody] FlyghtLinkedInUserModel model, int id)
        {
            try
            {
                if (model == null || id == 0)
                {
                    return BadRequest("Dados ou ID inválidos");
                }

                var result = await _flightService.PatchLinkedInUserFlyghts(model, id);
                return Ok(result);
            }
            catch
            {
                return StatusCode(500, "Erro ao atualizar voo monitorado.");
            }
        }
    }
}
