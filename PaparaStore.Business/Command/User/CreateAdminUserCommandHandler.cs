using AutoMapper;
using MediatR;

using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.Domain;
using PaparaStore.Data.Service;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;


namespace PaparaStore.Business.Command;

public class CreateAdminUserCommandHandler :
    IRequestHandler<CreateAdminUserCommand, ApiResponse<UserResponse>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IHashingService hashingService;

    public CreateAdminUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IHashingService hashingService)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.hashingService = hashingService;
    }

    public async Task<ApiResponse<UserResponse>> Handle(CreateAdminUserCommand request, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<UserRequest, User>(request.Request);
        mapped.Id = await GenerateUniqueIdAsync();
        mapped.Role = "admin";
        mapped.Password = hashingService.HashPassword(mapped.Password);
        mapped.Status = 1;

        await unitOfWork.UserRepository.Insert(mapped);
        await unitOfWork.Complete();
        var wallet = new Wallet
        {
            UserId = mapped.Id,
            Balance = 0,
            RewardPoints = 0
        };

        await unitOfWork.WalletRepository.Insert(wallet);
        await unitOfWork.Complete();
        var response = mapper.Map<UserResponse>(mapped);
        return new ApiResponse<UserResponse>(response);
    }
    private async Task<long> GenerateUniqueIdAsync()
    {
        long uniqueId;
        bool isUnique;

        do
        {
            Random random = new Random();
            uniqueId = random.Next(100, 999);

            isUnique = !await unitOfWork.UserRepository.AnyAsync(x => x.Id == uniqueId);

        } while (!isUnique);

        return uniqueId;
    }


}