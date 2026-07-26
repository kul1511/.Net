using AutoMapper;
using CROPDEAL.Models.DTO;
using CROPDEAL.Models;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<Crop, CropDTO>().ReverseMap();
        CreateMap<Order, OrderDTO>().ReverseMap();
        CreateMap<BankAccount, BankAccountDTO>().ReverseMap();
        CreateMap<Subscription, SubscriptionDTO>().ReverseMap();
        CreateMap<Payment, PaymentDTO>().ReverseMap();
        CreateMap<Invoice, InvoiceDTO>().ReverseMap();
    }
}
