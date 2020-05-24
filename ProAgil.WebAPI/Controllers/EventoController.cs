using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        public IProAgilRepository _repository { get; }
        public EventoController(IProAgilRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try{
                var results = await _repository.GetAllEventoAsync(true);
                return Ok(results);    
            } catch (System.Exception) {                
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Bando de Dados Falhou");                
            }            
        }

        [HttpGet("EventoId")]
        public async Task<IActionResult> Get(int EventoId)
        {
            try{
                var results = await _repository.GetEventoAsyncById(EventoId,true);
                return Ok(results);    
            } catch (System.Exception) {                
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Bando de Dados Falhou");                
            }            
        }

        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try{
                var results = await _repository.GetAllEventoAsyncByTema(tema,true);
                return Ok(results);    
            } catch (System.Exception) {                
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Bando de Dados Falhou");                
            }            
        }

        [HttpPost]
        public async Task<IActionResult> Post(Evento model)
        {
            try{
                _repository.Add(model);
                if(await _repository.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}",model);
                }                  
            } catch (System.Exception) {                
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Bando de Dados Falhou");                
            } 
            return BadRequest();           
        }   

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> Put(int eventoId, Evento model)
        {
            try{
                var evento = await _repository.GetEventoAsyncById(eventoId, false);
                if(evento == null){
                    return NotFound();
                }
                _repository.Update(model);
                if(await _repository.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}",model);
                }                  
            } catch (System.Exception) {                
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Bando de Dados Falhou");                
            } 
            return BadRequest();           
        }   

        [HttpDelete("{eventoId}")]
        public async Task<IActionResult> Delete(int eventoId)
        {
            try{
                var evento = await _repository.GetEventoAsyncById(eventoId, false);
                if(evento == null){
                    return NotFound();
                }
                _repository.Delete(evento);
                if(await _repository.SaveChangesAsync())
                {
                    return Ok();
                }                  
            } catch (System.Exception) {                
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Bando de Dados Falhou");                
            } 
            return BadRequest();           
        }  
    }
}