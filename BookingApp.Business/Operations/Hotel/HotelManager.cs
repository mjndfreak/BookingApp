using BookingApp.Business.Operations.Hotel.Dtos;
using BookingApp.Business.Types;
using BookingApp.Data.Entities;
using BookingApp.Data.Repositories;
using BookingApp.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Business.Operations.Hotel;

public class HotelManager : IHotelService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<HotelEntity> _hotelRepository;
    private readonly IRepository<HotelFeatureEntity> _hotelFeatureRepository;
    
    public HotelManager(IUnitOfWork unitOfWork, IRepository<HotelEntity> hotelRepository, IRepository<HotelFeatureEntity> hotelFeatureRepository)
    {
        _unitOfWork = unitOfWork;
        _hotelRepository = hotelRepository;
        _hotelFeatureRepository = hotelFeatureRepository;
    }

    public async Task<ServiceMessage> AddHotel(AddHotelDto hotelDto)
    {
        var hasHotel = _hotelRepository.GetAll(x => x.Name.ToLower() == hotelDto.Name.ToLower()).Any();

        if (hasHotel)
        {
            return new ServiceMessage
            {
                IsSuccess = false,
                Message = "This hotel is in database already."
            };
        }

        await _unitOfWork.BeginTransactionAsync();
        var hotelEntity = new HotelEntity
        {
            Name = hotelDto.Name,
            Stars = hotelDto.Stars,
            Address = hotelDto.Address,
            AccomodationType = hotelDto.AccomodationType
        };
        _hotelRepository.Add(hotelEntity);
        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw new Exception("Error while adding hotel!");
        }

        foreach (var featureId in hotelDto.FeatureIds)
        {
            var hotelFeature = new HotelFeatureEntity
            {
                HotelId = hotelEntity.Id,
                FeatureId = featureId
            };

            _hotelFeatureRepository.Add(hotelFeature);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                throw new Exception("Error while adding Hotel Feature!!");
            }

           
        }
        return new ServiceMessage
        { 
            IsSuccess = true
        };
    }

    public async Task<HotelDto> GetHotel(int id)
    {
        var hotel = await _hotelRepository.GetAll(x => x.Id == id).Select(x => new HotelDto
        {
            Id = x.Id,
            Name = x.Name,
            Stars = x.Stars,
            Address = x.Address,
            AccomodationType = x.AccomodationType,
            Features = x.HotelFeatures.Select(f => new HotelFeatureDto
            {
                Id = f.Feature.Id,
                Title = f.Feature.Title
            }).ToList()
        }).FirstOrDefaultAsync();
        return hotel;
    }

    public async Task<List<HotelDto>> GetAllHotels()
    {
        var hotels = await _hotelRepository.GetAll().Select(x => new HotelDto
        {
            Id = x.Id,
            Name = x.Name,
            Stars = x.Stars,
            Address = x.Address,
            AccomodationType = x.AccomodationType,
            Features = x.HotelFeatures.Select(f => new HotelFeatureDto
            {
                Id = f.Feature.Id,
                Title = f.Feature.Title
            }).ToList()
        }).ToListAsync();
        return hotels;
    }

    public async Task<ServiceMessage> AdjustHotelStars(int id, int changeTo)
    {
        var hotel = _hotelRepository.GetById(id);
        if (hotel is null)
        {
            return new ServiceMessage
            {
                IsSuccess = false,
                Message = "No hotel found with this Id!"
            };
        }
        hotel.Stars = changeTo;
        _hotelRepository.Update(hotel);
        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw new Exception("Error while updating hotel stars!");
        }
        return new ServiceMessage
        {
            IsSuccess = true
        };
    }

    public async Task<ServiceMessage> DeleteHotel(int id)
    {
        var hotel = _hotelRepository.GetById(id);
        if (hotel is null)
        {
            return new ServiceMessage
            {
                IsSuccess = false,
                Message = "No hotel found with this Id!"
            };
        }
        _hotelRepository.Delete(id);
        
        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw new Exception("Error while deleting hotel!");
        }
        return new ServiceMessage
        {
            IsSuccess = true
        };  
    }

    public async Task<ServiceMessage> UpdateHotel(UpdateHotelDto updateHotelDto)
    {
        var hotel = _hotelRepository.GetById(updateHotelDto.Id);
        if (hotel is null)
        {
            return new ServiceMessage
            {
                IsSuccess = false,
                Message = "No hotel found with this Id!"
            };
        }

        await _unitOfWork.BeginTransactionAsync();
        
        hotel.Name = updateHotelDto.Name;
        hotel.Stars = updateHotelDto.Stars;
        hotel.Address = updateHotelDto.Address;
        hotel.AccomodationType = updateHotelDto.AccomodationType;
        
        _hotelRepository.Update(hotel);
        
        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            throw new Exception("Error while updating hotel!");
        }

        var hotelFeatures = _hotelFeatureRepository.GetAll(x => x.HotelId == updateHotelDto.Id).ToList();
        foreach (var hotelFeature in hotelFeatures)
        {
            _hotelFeatureRepository.Delete(hotelFeature, false);
        }

        foreach (var featureId in updateHotelDto.FeatureIds)
        {
            var hotelFeature = new HotelFeatureEntity
            {
                HotelId = hotel.Id,
                FeatureId = featureId
            };
            _hotelFeatureRepository.Add(hotelFeature);
        }

        try
        {
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransaction();
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            throw new Exception("Error while updating hotel features!");
        }

        return new ServiceMessage
        {
            IsSuccess = true
        };
    }
}