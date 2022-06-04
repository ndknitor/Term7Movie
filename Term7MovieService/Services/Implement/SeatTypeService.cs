using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class SeatTypeService : ISeatTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISeatTypeRepository seatTypeRepo;
        private readonly IMapper _mapper;

        public SeatTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            seatTypeRepo = _unitOfWork.SeatTypeRepository;
            _mapper = mapper;
        }

        public async Task<SeatTypeListResponse> GetAllSeatTypeAsync()
        {
            IEnumerable<SeatTypeDto> list = await seatTypeRepo.GetAllSeatTypeAsync();

            return new SeatTypeListResponse
            {
                Message = Constants.MESSAGE_SUCCESS,
                SeatTypes = list
            };
        }

        public async Task<SeatTypeResponse> GetSeatTypeByIdAsync(int id)
        {
            SeatTypeDto dto = await seatTypeRepo.GetSeatTypeByIdAsync(id);

            if (dto == null) return null;

            return new SeatTypeResponse
            {
                Message = Constants.MESSAGE_SUCCESS,
                SeatType = dto
            };
        }

        public async Task<ParentResponse> UpdateSeatTypeAsync(SeatTypeUpdateRequest request)
        {
            SeatType seatType = _mapper.Map<SeatType>(request);
            await seatTypeRepo.UpdateSeatTypeAsync(seatType);

            if (_unitOfWork.HasChange())
            {
                await _unitOfWork.CompleteAsync();
                return new ParentResponse { Message = Constants.MESSAGE_SUCCESS };
            }

            return null;
        }
    }
}
