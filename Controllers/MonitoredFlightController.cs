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

        /// <summary>
        /// Obtém todos os voos monitorados externos.
        /// </summary>
        /// <returns>Uma lista de voos monitorados.</returns>
        /// <response code="200">Retorna a lista de voos monitorados.</response>
        /// <response code="404">Não foram encontrados voos.</response>
        [HttpGet("getAllExternal")]
        [SwaggerOperation(Summary = "Obtém todos os voos monitorados externos")]
        [SwaggerResponse(200, "Lista de voos monitorados", typeof(List<MonitoredFlightsModel>))]
        [SwaggerResponse(404, "Nenhum voo encontrado")]
        public async Task<List<MonitoredFlightsModel>> ShowAllFlights()
        {
            var flights = await _flightService.GetAllFlightsExternalApiData();

            if (flights == null || !flights.Any())
            {
                return new List<MonitoredFlightsModel>();
            }

            var response = flights.ToList();

            return response;
        }

        /// <summary>
        /// Obtém um voo monitorado externo pelo ID.
        /// </summary>
        /// <param name="id">ID do voo</param>
        /// <returns>Informações do voo monitorado.</returns>
        /// <response code="200">Retorna o voo monitorado com sucesso.</response>
        /// <response code="400">ID inválido.</response>
        [HttpGet("getOne/{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Obtém um voo monitorado pelo ID")]
        [SwaggerResponse(200, "Voo monitorado encontrado", typeof(MonitoredFlightsModel))]
        [SwaggerResponse(400, "ID inválido")]
        public async Task<ActionResult> GetOneFlightsExternalApiData(int id)
        {
            if (id == null)
            {
                return BadRequest("ID cannot be null");
            }

            var result = await _flightService.GetOneFlightsExternalApiData(id);
            return Ok(result);
        }

        /// <summary>
        /// Obtém todos os dados de voos internos monitorados.
        /// </summary>
        /// <returns>Uma lista de voos monitorados internos.</returns>
        [HttpGet("getAllinternal")]
        [Authorize]
        [SwaggerOperation(Summary = "Obtém todos os dados de voos internos monitorados")]
        [SwaggerResponse(200, "Lista de voos internos monitorados", typeof(IEnumerable<MonitoredFlightsModel>))]
        public async Task<ActionResult> GetAllFlightInternalData()
        {
            var result = await _flightService.GetAllFlightInternalData();
            return Ok(result);
        }

        /// <summary>
        /// Cria um novo voo monitorado interno.
        /// </summary>
        /// <param name="model">Dados do voo monitorado a ser criado</param>
        /// <returns>Informações do voo monitorado criado.</returns>
        /// <response code="201">Voo criado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpPost("postInternal")]
        [Authorize]
        [SwaggerOperation(Summary = "Cria um novo voo monitorado interno")]
        [SwaggerResponse(201, "Voo monitorado criado com sucesso", typeof(MonitoredFlightsModel))]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<ActionResult> CreateRegisterMonitoredFlyght([FromBody] FlyghtLinkedInUserModel model)
        {
            if (model == null)
            {
                return BadRequest("Model cannot be null");
            }

            var result = await _flightService.CreateRegisterMonitoredFlyght(model);
            return Ok(result);
        }

        /// <summary>
        /// Deleta um voo monitorado interno pelo ID.
        /// </summary>
        /// <param name="id">ID do voo a ser deletado</param>
        /// <returns>Status da operação.</returns>
        /// <response code="200">Voo deletado com sucesso.</response>
        /// <response code="400">ID inválido.</response>
        [HttpDelete("deleteInternal/{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Deleta um voo monitorado interno pelo ID")]
        [SwaggerResponse(200, "Voo deletado com sucesso")]
        [SwaggerResponse(400, "ID inválido")]
        public async Task<ActionResult> DeleteRegisterLinkedInUserFlyghts(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid Id");
            }

            var result = await _flightService.DeleteRegisterLinkedInUserFlyghts(id);
            return Ok(result);
        }

        /// <summary>
        /// Atualiza um voo monitorado interno.
        /// </summary>
        /// <param name="model">Dados atualizados do voo monitorado</param>
        /// <param name="id">ID do voo a ser atualizado</param>
        /// <returns>Informações do voo atualizado.</returns>
        /// <response code="200">Voo atualizado com sucesso.</response>
        /// <response code="400">ID ou dados inválidos.</response>
        [HttpPatch("patchInternal/{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Atualiza um voo monitorado interno")]
        [SwaggerResponse(200, "Voo atualizado com sucesso", typeof(MonitoredFlightsModel))]
        [SwaggerResponse(400, "ID ou dados inválidos")]
        public async Task<ActionResult> PatchLinkedInUserFlyghts([FromBody] FlyghtLinkedInUserModel model, int id)
        {
            if (model == null)
            {
                return BadRequest("Invalid data");
            }

            if (id == 0)
            {
                return BadRequest("Invalid Id");
            }

            var result = await _flightService.PatchLinkedInUserFlyghts(model, id);
            return Ok(result);
        }
    }
}
