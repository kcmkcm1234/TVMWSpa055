using UserInterface.Models;
using SPAccounts.DataAccessObject.DTO;
using SAMTool.DataAccessObject.DTO;

namespace UserInterface.App_Start
{
    public class MappingConfig
    {
        public static void RegisterMaps()
        {
            AutoMapper.Mapper.Initialize(config =>
            {
                //domain <===== viewmodel
                //viewmodel =====> domain
                //ReverseMap() makes it possible to map both ways.


                //*****SAMTOOL MODELS 
                config.CreateMap<LoginViewModel, User>().ReverseMap();
                config.CreateMap<UserViewModel, User>().ReverseMap();
                config.CreateMap<HomeViewModel, Home>().ReverseMap();
                config.CreateMap<RolesViewModel, Roles>().ReverseMap();
                config.CreateMap<ApplicationViewModel, Application>().ReverseMap();
                config.CreateMap<AppObjectViewModel, AppObject>().ReverseMap();
                config.CreateMap<AppSubobjectViewmodel, AppSubobject>().ReverseMap();
                config.CreateMap<CommonViewModel, SAMTool.DataAccessObject.DTO.Common>().ReverseMap();

                //****SAMTOOL MODELS 




                //****SPACCOUNTS MODELS 
                config.CreateMap<MenuViewModel, Menu>().ReverseMap();
                config.CreateMap<CustomerInvoiceSummaryViewModel, CustomerInvoiceSummary>().ReverseMap();
                config.CreateMap<CustomerInvoicesViewModel, CustomerInvoice>().ReverseMap();
                config.CreateMap<CustomerViewModel, Customer>().ReverseMap();
                config.CreateMap<CommonViewModel, SPAccounts.DataAccessObject.DTO.Common>().ReverseMap();
                config.CreateMap<PaymentTermsViewModel, PaymentTerms>().ReverseMap();
                config.CreateMap<CompaniesViewModel, Companies>().ReverseMap();
                config.CreateMap<TaxTypesViewModel, TaxTypes>().ReverseMap();
                config.CreateMap<SuppliersViewModel, Supplier>().ReverseMap();
                config.CreateMap<CustomerCreditNoteViewModel, CustomerCreditNotes>().ReverseMap();
                config.CreateMap<BankViewModel, Bank>().ReverseMap();
                config.CreateMap<TaxTypesViewModel, TaxTypes>().ReverseMap();
                config.CreateMap<PaymentModesViewModel, PaymentModes>().ReverseMap();
            });
        }
    }
}