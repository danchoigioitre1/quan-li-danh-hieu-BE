using MISA.FRESHER032023.COMMON.Enums;
using MISA.FRESHER032023.COMMON.Exceptions;
using MISA.FRESHERWEB032023.BL.Dto;
using MISA.FRESHERWEB032023.BL.Services.EmulationService;
using MISA.FRESHERWEB032023.DL.Entity.Emualtion;
using MISA.FRESHERWEB032023.DL.Repository.EmulationRepo;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MISA.FRESHERWEB032023.UNITTEST.Services
{
    [TestFixture]
    public class EmulationServiceTests
    {
        [Test]
        public async Task GetAsync_NotFound_ReturnException()
        {
            // Arrange
            var id = Guid.Parse("feb20223-f90c-11ed-b95a-d8bbc1208bca");

            var emulationRepo = Substitute.For<IEmulationRepository>();

            emulationRepo.GetAsync(id).ReturnsNull();

            var emulationService = new EmulationService(emulationRepo);

            //Act
            
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await emulationService.GetAsync(id));
            Assert.That(ex.Message, Is.EqualTo("Không tìm thấy bản ghi"));
            //Assert
        }

        [Test]

        public async Task GetAsync_ValidInput_ReturnEmulation()
        {
            // Arrange
            var id = Guid.Parse("feb20223-f90c-11ed-b95a-d8bbc1208bca");

            var emulationRepo = Substitute.For<IEmulationRepository>();
            var emulation = new EmulationBase
            {
                EmulationCode = "code",
                EmulationId = id,
                EmulationLevel = (FRESHER032023.COMMON.Enums.EmulationLevel)1,
                EmulationName = "name",
                EmulationStatus = EmulationStatus.Use,
                EmulationTarget = (FRESHER032023.COMMON.Enums.EmulationTarget)1,
                EmulationType = (FRESHER032023.COMMON.Enums.EmulationType)1
            };

            var emulationDto = new EmulationDto
            {
                EmulationCode = "code",
                EmulationId = id,
                EmulationLevel = (FRESHER032023.COMMON.Enums.EmulationLevel)1,
                EmulationName = "name",
                EmulationStatus = (FRESHER032023.COMMON.Enums.EmulationStatus)1,
                EmulationTarget = (FRESHER032023.COMMON.Enums.EmulationTarget)1,
                EmulationType = (FRESHER032023.COMMON.Enums.EmulationType)1
            };
            emulationRepo.GetAsync(id).Returns(emulation);

            var emulationService = new EmulationService(emulationRepo);

            //Act
            var actualResult = await emulationService.GetAsync(id);

            //Assert
            Assert.That(actualResult, Is.EqualTo(emulationDto));

        }

        [Test]
        public async Task DeleteAsync_NotFound_ReturnException()
        {
            // Arrange
            var id = Guid.Parse("feb20223-f90c-11ed-b95a-d8bbc1208bca");

            var emulationRepo = Substitute.For<IEmulationRepository>();

            emulationRepo.GetAsync(id).ReturnsNull();

            var emulationService = new EmulationService(emulationRepo);

            //Act vs Assert

            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await emulationService.DeleteAsync(id));
            Assert.That(ex.Message, Is.EqualTo("Không tìm thấy dữ liệu !"));


        }

        [Test]
        public async Task DeleteAsync_ValidInput_DeleteSuccess()
        {
            // Arrange
            var id = Guid.Parse("feb20223-f90c-11ed-b95a-d8bbc1208bca");

            var emulationRepo = Substitute.For<IEmulationRepository>();
            var emulation = new EmulationBase()
            {
                EmulationCode = "code",
                EmulationId = id,
                EmulationLevel = EmulationLevel.All,
                EmulationName = "name",
                EmulationStatus = (FRESHER032023.COMMON.Enums.EmulationStatus)1,
                EmulationTarget = (FRESHER032023.COMMON.Enums.EmulationTarget)1,
                EmulationType = (FRESHER032023.COMMON.Enums.EmulationType)1

            };

            emulationRepo.GetAsync(id).Returns(emulation);
            emulationRepo.DeleteAsync(id).Returns(1);
            var emulationService = new EmulationService(emulationRepo);

            // Act

            await emulationService.DeleteAsync(id);

            // Assert
            await emulationRepo.Received(1).DeleteAsync(id);


        }
        [Test]
        public async Task DeleteAsync_StatusUse_ReturnException()
        {
            var id = Guid.Parse("feb20223-f90c-11ed-b95a-d8bbc1208bca");

            var emulationRepo = Substitute.For<IEmulationRepository>();
            var emulation = new EmulationBase()
            {
                EmulationCode = "code",
                EmulationId = id,
                EmulationLevel = (FRESHER032023.COMMON.Enums.EmulationLevel)1,
                EmulationName = "name",
                EmulationStatus = (FRESHER032023.COMMON.Enums.EmulationStatus)1,
                EmulationTarget = (FRESHER032023.COMMON.Enums.EmulationTarget)1,
                EmulationType = (FRESHER032023.COMMON.Enums.EmulationType)1

            };

            emulationRepo.GetAsync(id).Returns(emulation);
            emulationRepo.DeleteAsync(id).Returns(1);
            var emulationService = new EmulationService(emulationRepo);

            //Act vs Assert

            var ex = Assert.ThrowsAsync<Exception>(async () => await emulationService.DeleteAsync(id));
            Assert.That(ex.Message, Is.EqualTo("Đang sử dụng nên không được xóa"));

        }

    }
}
