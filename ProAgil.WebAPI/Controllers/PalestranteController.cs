using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PalestranteController : ControllerBase
    {
        public IProAgilRepository _repository { get; }
        public PalestranteController(IProAgilRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("PalestranteId")]
        public async Task<IActionResult> Get(int PalestranteId)
        {
            try{
                var results = await _repository.GetPalestranteAsync(PalestranteId, true);
                return Ok(results);    
            } catch (System.Exception) {                
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Bando de Dados Falhou");                
            }            
        }

        [HttpGet("getByName/{name}")]
        public async Task<IActionResult> Get(string name)
        {
            try{
                var results = await _repository.GetAllPalestrantesAsyncByName(name,true);
                return Ok(results);    
            } catch (System.Exception) {                
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Bando de Dados Falhou");                
            }            
        }

        [HttpPost]
        public async Task<IActionResult> Post(Palestrante model)
        {
            try{
                _repository.Add(model);
                if (await _repository.SaveChangesAsync())
                {
                    return Created($"/api/palestrante/{model.Id}",model);
                }                  
            } catch (System.Exception) {                
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Bando de Dados Falhou");                
            } 
            return BadRequest();           
        }   

        [HttpPut]
        public async Task<IActionResult> Put(int palestranteId, Palestrante model)
        {
            try{
                var palestrante = await _repository.GetEventoAsyncById(palestranteId, false);
                if (palestrante == null){
                    return NotFound();
                }
                _repository.Update(model);
                if (await _repository.SaveChangesAsync())
                {
                    return Created($"/api/palestrante/{model.Id}",model);
                }                  
            } catch (System.Exception) {                
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Bando de Dados Falhou");                
            } 
            return BadRequest();           
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int palestranteId)
        {
            try{
                var palestrante = await _repository.GetEventoAsyncById(palestranteId, false);
                if (palestrante == null){
                    return NotFound();
                }
                _repository.Delete(palestrante);
                if (await _repository.SaveChangesAsync())
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