using OkanDemir.Business.Encyription;
using OkanDemir.Data.Repository;
using OkanDemir.Model;

namespace OkanDemir.Business.Services
{
    public class BackgroundServices
    {
        private readonly IRepository<Note> _noteRepository;
        private readonly IRepository<User> _userRepository;

        public BackgroundServices(IRepository<Note> _noteRepository,
            IRepository<User> _userRepository)
        {
            this._noteRepository = _noteRepository;
            this._userRepository = _userRepository;
        }

        public void Execute()
        {
            var alertNotes = _noteRepository.ListQueryable
                .Where(x => x.IsAlert && !x.SendSms && x.AlertTime <= DateTime.Now).ToList();

            //telefon þifrelendi.

            //foreach (var item in alertNotes)
            //{
            //    var phone = _userRepository.ListQueryableNoTracking.FirstOrDefault(x=>x.Id == item.UserId).Phone;

            //    SmsService helper = new SmsService();
            //    helper.SendMessage(item.Id + " idli notu incelemen gerekiyor alarm kurmuþsun unutma bak ona", phone);

            //    item.SendSms = true;
            //    _noteRepository.Update(item);
            //}
        }
    }

}