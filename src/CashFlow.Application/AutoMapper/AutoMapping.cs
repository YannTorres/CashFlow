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
        CreateMap<Expense, ResponseExpenseJson>()
            .ForMember(dest => dest.Tags, config => 
                config.MapFrom(source => source.Tags.Select(tag => tag.Value)));
        // A nossa response é uma lista de enuns e a expense é uma lista e entidades
        // Assim a gente mapeia o valor que esta dentro da entidade para a response.

        CreateMap<Expense, ResponseRegisteredExpenseJson>();
        CreateMap<Expense, ResponseShortExpenseJson>();
        CreateMap<User, ResponseUserProfileJson>();
    }
}
