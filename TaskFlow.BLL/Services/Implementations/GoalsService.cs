using TaskFlow.BLL.DTOs;
using TaskFlow.BLL.Mappings;
using TaskFlow.BLL.Services.Interfaces;
using TaskFlow.DAL.Entities;
using TaskFlow.DAL.Repositories.Interfaces;

namespace TaskFlow.BLL.Services.Implementations
{
    public class GoalsService : IGoalsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GoalsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GoalDto> CreateAsync(GoalCreateDto dto, CancellationToken cancellationToken = default)
        {
            if (dto.TargetDate.Date <= DateTime.UtcNow)
                throw new ArgumentException("Target date must be in the future.", nameof(dto.TargetDate));

            var entity = dto.ToEntity();
            await _unitOfWork.GoalsRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Map(entity);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.GoalsRepository.GetByIdAsync(id, cancellationToken);
            if (entity is null) throw new KeyNotFoundException();

            _unitOfWork.GoalsRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<List<GoalDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
            (await _unitOfWork.GoalsRepository.GetAllAsync(cancellationToken))
                .Select(Map)
                .ToList();

        private static GoalDto Map(GoalEntity entity) =>
            entity.ToDto();
    }
}
