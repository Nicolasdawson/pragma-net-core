using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Moq;
using Pragma.Application.Api.Controllers;
using Pragma.Application.Domain.Entities;
using Pragma.Application.Domain.Models;
using Pragma.Application.Infrastructure.Repository;
using System.Linq.Expressions;

namespace Pragma.Tests.Controllers
{
    public class UsuariosControllerTests
    {
        private MockRepository _mockRepository;

        private Mock<IRepository<Usuario>> _mockUsuarioRepository;
        private Mock<ILogger<UsuariosController>> _mockLogger;

        public UsuariosControllerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Default);

            _mockUsuarioRepository = _mockRepository.Create<IRepository<Usuario>>();
            _mockLogger = _mockRepository.Create<ILogger<UsuariosController>>();
        }

        private UsuariosController CreateUsuariosController()
        {
            var controllerInstance = new UsuariosController(_mockUsuarioRepository.Object, _mockLogger.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            };

            controllerInstance.ControllerContext.HttpContext = new DefaultHttpContext();
            controllerInstance.ControllerContext.HttpContext.Request.Path = "/users";
            controllerInstance.ControllerContext.HttpContext.Request.PathBase = string.Empty;
            controllerInstance.ControllerContext.HttpContext.Request.Scheme = "https";
            controllerInstance.ControllerContext.HttpContext.Request.Host = new HostString("localhost");

            return controllerInstance;
        }

        [Fact]
        public void GetUsuarios_StateUnderTest_ExpectedBehavior()
        {

            _mockUsuarioRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), null))
                .Returns(GetUsuarios().AsQueryable());

            var usuariosController = CreateUsuariosController();
            string search = null;

            var result = usuariosController.GetUsuarios(
                search,
                0,
                0);

            Assert.NotNull(result);
            var usuarios = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.True(usuarios.StatusCode == 200);
            Assert.NotNull(usuarios.Value);
        }

        [Fact]
        public void GetUsuarios_StateUnderTest_search_ExpectedBehavior()
        {

            _mockUsuarioRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), null))
                .Returns(GetUsuarios().AsQueryable());

            var usuariosController = CreateUsuariosController();
            string search = "Juanito";

            var result = usuariosController.GetUsuarios(
                search,
                0,
                0);

            Assert.NotNull(result);
            var usuarios = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.True(usuarios.StatusCode == 200);
            Assert.NotNull(usuarios.Value);
        }

        [Fact]
        public void GetUsuarios_StateUnderTest_paginacion_ExpectedBehavior()
        {

            _mockUsuarioRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), null))
                .Returns(GetUsuarios().AsQueryable());

            var usuariosController = CreateUsuariosController();
            string search = "Juanito";

            var result = usuariosController.GetUsuarios(
                search,
                1,
                1);

            Assert.NotNull(result);
            var usuarios = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.True(usuarios.StatusCode == 200);
            Assert.NotNull(usuarios.Value);
        }

        [Fact]
        public void GetUsuario_StateUnderTest_ExpectedBehavior()
        {

            _mockUsuarioRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), null))
                .Returns(GetUsuarios().AsQueryable());

            var usuariosController = CreateUsuariosController();

            var id = Guid.NewGuid();

            var result = usuariosController.GetUsuario(id);

            Assert.NotNull(result);
            var usuarios = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.True(usuarios.StatusCode == 200);
            Assert.NotNull(usuarios.Value);
        }

        [Fact]
        public void GetUsuario_StateUnderTest_invalid_guid_ExpectedBehavior()
        {
            _mockUsuarioRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), null))
                .Returns(GetUsuarios().AsQueryable());

            var usuariosController = CreateUsuariosController();

            var id = Guid.Empty;

            var result = usuariosController.GetUsuario(id);

            Assert.NotNull(result);
            var usuarios = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.True(usuarios.StatusCode == 400);
            Assert.NotNull(usuarios.Value);
        }

        [Fact]
        public void GetUsuario_StateUnderTest_no_user_ExpectedBehavior()
        {
            var usuariosController = CreateUsuariosController();

            var id = Guid.NewGuid();

            var result = usuariosController.GetUsuario(id);

            Assert.NotNull(result);
            var usuarios = Assert.IsAssignableFrom<NotFoundObjectResult>(result);
            Assert.True(usuarios.StatusCode == 404);
            Assert.NotNull(usuarios.Value);
        }

        [Fact]
        public void CreateUsuario_StateUnderTest_ExpectedBehavior()
        {
            var entity = GetUsuarios().First();

            var request = GetUsuarioRequest();

            _mockUsuarioRepository.Setup(x => x.Create(entity)).Returns(entity);

            var usuariosController = CreateUsuariosController();

            var result = usuariosController.CreateUsuario(request);

            Assert.NotNull(result);
            var usuarios = Assert.IsAssignableFrom<CreatedResult>(result);
            Assert.True(usuarios.StatusCode == 201);
        }

        [Fact]
        public void CreateUsuario_StateUnderTest_invalid_rut_ExpectedBehavior()
        {
            var entity = GetUsuarios().First();

            var request = GetUsuarioRequest();

            request.Rut = "19837447-3";

            _mockUsuarioRepository.Setup(x => x.Create(entity)).Returns(entity);

            var usuariosController = CreateUsuariosController();

            var result = usuariosController.CreateUsuario(request);

            Assert.NotNull(result);
            var usuarios = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.True(usuarios.StatusCode == 400);
            Assert.NotNull(usuarios.Value);
        }

        [Fact]
        public void UpdateUsuario_StateUnderTest_ExpectedBehavior()
        {
            var entity = GetUsuarios().First();

            _mockUsuarioRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), null))
                .Returns(GetUsuarios().AsQueryable());

            _mockUsuarioRepository.Setup(x => x.Update(entity)).Returns(entity);

            var usuariosController = CreateUsuariosController();

            var id = Guid.NewGuid();
            var request = GetUsuarioRequest();

            var result = usuariosController.UpdateUsuario(request, id);

            Assert.NotNull(result);
            var usuarios = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.True(usuarios.StatusCode == 200);
        }

        [Fact]
        public void UpdateUsuario_StateUnderTest_invalid_guid_ExpectedBehavior()
        {
            var usuariosController = CreateUsuariosController();

            var id = Guid.Empty;
            var request = GetUsuarioRequest();

            var result = usuariosController.UpdateUsuario(request, id);

            Assert.NotNull(result);
            var usuarios = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.True(usuarios.StatusCode == 400);
            Assert.NotNull(usuarios.Value);
        }

        [Fact]
        public void UpdateUsuario_StateUnderTest_invalid_rut_ExpectedBehavior()
        {
            var usuariosController = CreateUsuariosController();

            var id = Guid.NewGuid();
            var request = GetUsuarioRequest();

            request.Rut = "19837447-3";

            var result = usuariosController.UpdateUsuario(request, id);

            Assert.NotNull(result);
            var usuarios = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.True(usuarios.StatusCode == 400);
            Assert.NotNull(usuarios.Value);
        }

        [Fact]
        public void UpdateUsuario_StateUnderTest_no_user_ExpectedBehavior()
        {
            var entity = GetUsuarios().First();

            _mockUsuarioRepository.Setup(x => x.Update(entity)).Returns(entity);

            var usuariosController = CreateUsuariosController();

            var id = Guid.NewGuid();
            var request = GetUsuarioRequest();

            var result = usuariosController.UpdateUsuario(request, id);

            Assert.NotNull(result);
            var usuarios = Assert.IsAssignableFrom<NotFoundObjectResult>(result);
            Assert.True(usuarios.StatusCode == 404);
            Assert.NotNull(usuarios.Value);
        }

        [Fact]
        public void DeleteUsuario_StateUnderTest_ExpectedBehavior()
        {
            var entity = GetUsuarios().First();

            _mockUsuarioRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), null))
                .Returns(GetUsuarios().AsQueryable());

            _mockUsuarioRepository.Setup(x => x.Remove(entity));

            var usuariosController = CreateUsuariosController();

            var id = Guid.NewGuid();

            var result = usuariosController.DeleteUsuario(id);

            Assert.NotNull(result);
            var usuarios = Assert.IsAssignableFrom<NoContentResult>(result);
            Assert.True(usuarios.StatusCode == 204);
        }

        [Fact]
        public void DeleteUsuario_StateUnderTest_guid_invalid_ExpectedBehavior()
        {
            var entity = GetUsuarios().First();

            _mockUsuarioRepository.Setup(x => x.Remove(entity));

            var usuariosController = CreateUsuariosController();

            var id = Guid.Empty;

            var result = usuariosController.DeleteUsuario(id);

            Assert.NotNull(result);
            var usuarios = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.True(usuarios.StatusCode == 400);
        }

        [Fact]
        public void DeleteUsuario_StateUnderTest_no_user_ExpectedBehavior()
        {
            var entity = GetUsuarios().First();

            _mockUsuarioRepository.Setup(x => x.Remove(entity));

            var usuariosController = CreateUsuariosController();

            var id = Guid.NewGuid();

            var result = usuariosController.DeleteUsuario(id);

            Assert.NotNull(result);
            var usuarios = Assert.IsAssignableFrom<NotFoundObjectResult>(result);
            Assert.True(usuarios.StatusCode == 404);
        }

        private IList<Usuario> GetUsuarios() => new List<Usuario>
        {
            new Usuario
            {
                Correo = "juanito@gmail.com",
                FechaNacimiento = System.DateTime.Now,
                Id = Guid.NewGuid(),
                Nombre = "Juanito",
                Rut = "14502422-6"
            },
            new Usuario
            {
                Correo = "juanita@gmail.com",
                FechaNacimiento = System.DateTime.Now,
                Id = Guid.NewGuid(),
                Nombre = "Juanita",
                Rut = "4501326-K"
            }
        };

        private UsuarioRequest GetUsuarioRequest() => new UsuarioRequest
        {
            Correo = "juanito@gmail.com",
            FechaNacimiento = System.DateTime.Now,
            Nombre = "Juanito",
            Rut = "14502422-6"
        };
    }
}
