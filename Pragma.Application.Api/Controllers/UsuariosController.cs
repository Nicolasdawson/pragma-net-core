using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Linq.Expressions;
using Pragma.Application.Domain.Entities;
using Pragma.Application.Domain.Helpers;
using Pragma.Application.Domain.Models;
using Pragma.Application.Domain.Specification;
using Pragma.Application.Infrastructure.Repository;
using Microsoft.AspNetCore.Http.Extensions;

namespace Pragma.Application.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IRepository<Usuario> _usuarioRepository;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(IRepository<Usuario> usuarioRepository, ILogger<UsuariosController> logger)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        [HttpGet]
        [Route("usuarios")]
        public virtual IActionResult GetUsuarios(
            [FromQuery] string search,
            [FromQuery] int skip,
            [FromQuery] int take)
        {
            try
            {
                var queryUsuarios = _usuarioRepository.Get(null);

                if (!string.IsNullOrEmpty(search))
                {
                    queryUsuarios = UsuarioSpec.SearchQuery(queryUsuarios, search.Trim().ToLower());
                }

                //string sortColumn = HttpContext.Request.Query["sortColumn"];
                //string sortOrder = HttpContext.Request.Query["sortOrder"];

                //if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortOrder))
                //{
                //    if (sortOrder != "asc" && sortOrder != "desc")
                //    {
                //        return BadRequest("parametros de query invalidos: " + sortColumn);
                //    }

                //    var param = Expression.Parameter(typeof(Usuario), "p");
                //    var sortProperty = Expression.Property(param, sortColumn);
                //    var sortLambda = Expression.Lambda(sortProperty, param);

                //    var xd = x => x.Key;

                //    if (sortOrder == "asc")
                //    {
                //        queryUsuarios = queryUsuarios.OrderBy(x => x == sortOrder);
                //    }
                //    else
                //    {
                //        queryUsuarios = queryUsuarios.OrderByDescending(sortLambda);
                //    }
                //}

                if (skip == default && take == default)
                {
                    skip = 0;
                    take = 50;
                }

                return Ok(queryUsuarios.Skip(skip).Take(take).ToList());
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dex)
            {
                throw dex;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("usuarios/{id}")]
        public virtual IActionResult GetUsuario(
            [FromRoute] System.Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    var badRequestResponse = "Id de usuario invalido";
                    _logger.LogError(badRequestResponse);
                    return BadRequest(badRequestResponse);
                }

                var usuario = _usuarioRepository.Get(x => x.Id == id).FirstOrDefault();

                if (usuario == null)
                {
                    var notFoundResponse = $"usuario con id {id} no existe";
                    _logger.LogError(notFoundResponse);
                    return NotFound(notFoundResponse);
                }

                return Ok(usuario);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dex)
            {
                throw dex;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("usuarios")]
        public virtual IActionResult CreateUsuario(
            [FromBody] UsuarioRequest request)
        {
            try
            {
                var isRutValid = RutValidator.ValidaRut(request.Rut);

                if (!isRutValid)
                {
                    var badRequestResponse = "Rut invalido";
                    _logger.LogError(badRequestResponse);
                    return BadRequest(badRequestResponse);
                }

                Usuario usuario = request.ToEntity();

                var usuarioResponse = _usuarioRepository.Create(usuario);

                return Created(Request.GetDisplayUrl(), usuarioResponse);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dex)
            {
                throw dex;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        [Route("usuarios/{id}")]
        public virtual IActionResult UpdateUsuario(
            [FromBody] UsuarioRequest request,
            [FromRoute] System.Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    var badRequestResponse = "Id de usuario invalido";
                    _logger.LogError(badRequestResponse);
                    return BadRequest(badRequestResponse);
                }

                var isRutValid = RutValidator.ValidaRut(request.Rut);

                if (!isRutValid)
                {
                    var badRequestResponse = "Rut invalido";
                    _logger.LogError(badRequestResponse);
                    return BadRequest(badRequestResponse);
                }

                var usuario = _usuarioRepository.Get(x => x.Id == id).FirstOrDefault();

                if (usuario == null)
                {
                    var notFoundResponse = $"usuario con id {id} no existe";
                    _logger.LogError(notFoundResponse);
                    return NotFound(notFoundResponse);
                }

                request.ToUpdate(usuario);

                usuario = _usuarioRepository.Update(usuario);

                return Ok(usuario);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dex)
            {
                throw dex;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        [Route("usuarios/{id}")]
        public virtual IActionResult DeleteUsuario(
            [FromRoute] System.Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    var badRequestResponse = "Id de usuario invalido";
                    _logger.LogError(badRequestResponse);
                    return BadRequest(badRequestResponse);
                }

                var usuario = _usuarioRepository.Get(x => x.Id == id).FirstOrDefault();

                if (usuario == null)
                {
                    var notFoundResponse = $"usuario con id {id} no existe";
                    _logger.LogError(notFoundResponse);
                    return NotFound(notFoundResponse);
                }

                _usuarioRepository.Remove(usuario);

                return NoContent();

            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dex)
            {
                throw dex;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
