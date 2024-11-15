using BookingApp.Data.Entities;
using BookingApp.Data.Repositories;
using BookingApp.Data.UnitOfWork;

namespace BookingApp.Business.Operations.Setting;


    public class SettingManager : ISettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SettingEntity> _settingRepository;

        public SettingManager(IUnitOfWork unitOfWork, IRepository<SettingEntity> settingRepository)
        {
            _unitOfWork = unitOfWork;
            _settingRepository = settingRepository;
        }

        public async Task ToggleMaintenanceMode()
        {
            var setting = _settingRepository.GetById(1);
            setting.MaintenenceMode = !setting.MaintenenceMode;
            _settingRepository.Update(setting);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while updating the Maintanence Setting.");
            }
        }

        public bool GetMaintanenceStatus()
        {
            var maintanenceStatus = _settingRepository.GetById(1).MaintenenceMode;
            return maintanenceStatus;
        }
    
    
    }