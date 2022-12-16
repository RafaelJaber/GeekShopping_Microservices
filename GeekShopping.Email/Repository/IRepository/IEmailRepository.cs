using GeekShopping.Email.Messages;

namespace GeekShopping.Email.Repository.IRepository {
    public interface IEmailRepository {
        
        Task LogEmail(UpdatePaymentResultMessage message);
    }
}
