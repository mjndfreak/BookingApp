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
                IsSuccess = true,
                Message = "This hotel is in database already."
            };
        }

        await _unitOfWork.BeginTransactionAsync();
        var hotelEntity = new HotelEntity
        {
            Name = hotelDto.Name,
            Stars = hotelDto.Stars,
            Location = hotelDto.Location,
            AccomodationType = hotelDto.AccomodationType
        };
        _hotelRepository.Add(hotelEntity);
        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
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
                throw new Exception("Error while adding Hotel Featur!!");
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
            Location = x.Location,
            AccomodationType = x.AccomodationType,
            Features = x.HotelFeatures.Select(x => new HotelFeatureDto
            {
                Id = x.Feature.Id,
                Title = x.Feature.Title
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
            Location = x.Location,
            AccomodationType = x.AccomodationType,
            Features = x.HotelFeatures.Select(x => new HotelFeatureDto
            {
                Id = x.Feature.Id,
                Title = x.Feature.Title
            }).ToList()
        }).ToListAsync();
        return hotels;
    }
}