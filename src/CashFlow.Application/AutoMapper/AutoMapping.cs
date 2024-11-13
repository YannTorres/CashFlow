using AutoMapper;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Response;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.AutoMapper;
public class AutoMapping: Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }

    private void RequestToEntity()
    {
        
        CreateMap<RequestRegisteredUserJson, User>()
            .ForMember(dest => dest.Password, config => config.Ignore());

        CreateMap<RequestExpenseJson, Expense>()
            .ForMember(dest => dest.Tags, config => 
                config.MapFrom(source => source.Tags.Distinct()));
        // Para não permitir que tags repetidas sejam adicionadas.

        CreateMap<Communication.Enums.Tag, Domain.Entities.Tag>()
            .ForMember(dest => dest.Value, config => 
                config.MapFrom(source => source));
        // Com esse código mapeamos de uma list de enum para list da entidade.
    }
    private void EntityToResponse()
    {
        CreateMap<Expense, ResponseRegisteredExpenseJson>();
        CreateMap<Expense, ResponseShortExpenseJson>();
        CreateMap<Expense, ResponseExpenseJson>();
        CreateMap<User, ResponseUserProfileJson>();
    }
}
