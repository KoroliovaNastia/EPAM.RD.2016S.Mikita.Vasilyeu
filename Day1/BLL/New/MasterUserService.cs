using BLL;
using DAL;
using DAL.Interface;
using System;
using System.Diagnostics;
using System.Linq;
using BLL.Models;
using BLL.Mappers;

namespace BLL
{
    public class MasterUserService : BaseUserService
    {
        public event EventHandler<UserDataApdatedEventArgs> Deleted;
        public event EventHandler<UserDataApdatedEventArgs> Added;
        public MasterUserService(IUserRepository repository)
            :base(repository)
        {

        }

        public MasterUserService()
                :this(new UserRepository())
        {
            
        }

        protected override int AddStrategy(BllUser user)
        {
            //Console.WriteLine("AddStrategy: " + AppDomain.CurrentDomain.FriendlyName);

            //var errorMessages = Validator.Validate(user).ToList();
            //if (errorMessages.Any())
            //{
            //    TraceSource.TraceEvent(TraceEventType.Error, 0, $"Validation ERROR on User: {user.LastName} {user.PersonalId}\n " +
            //                                                    "ValidationMessages: " + string.Join("\n",errorMessages));
            //    throw new ArgumentException("Validation error! User is not valid\nValidationMessages: " + string.Join("\n", errorMessages));
            //}
            //if (UserExists(user))
            //{
            //    throw new ArgumentException("That User was added before!");
            //}
            //user.Id = NumGenerator.GenerateId();
            storage.Add(user.ToDalUser());
            OnUserAdded(this, new UserDataApdatedEventArgs {User = user});
            return user.Id;
        }

        protected override void DeleteStrategy(BllUser user)
        {
            storage.Delete(user.ToDalUser());
            OnUserDeleted(this, new UserDataApdatedEventArgs { User = user });
        }

        public override void Save()
        {
            //if (LoggingEnabled)
            //    TraceSource.TraceEvent(TraceEventType.Information, 0, "Saving UserService state...");

            //Repository.Save(LastGeneratedId);
        }

        public override void Initialize()
        {
            //if (LoggingEnabled)
            //    TraceSource.TraceEvent(TraceEventType.Information, 0, "Initializing UserService state...");

            //Repository.Initialize();
            //LastGeneratedId = Repository.GetState();
        }

        protected virtual void OnUserDeleted(object sender, UserDataApdatedEventArgs args)
        {
            //if (LoggingEnabled)
            //    TraceSource.TraceEvent(TraceEventType.Information, 0, $"User {args.User.LastName} {args.User.PersonalId} was deleted");
            Communicator?.SendDelete(args);
            Deleted?.Invoke(sender, args);
        }

        protected virtual void OnUserAdded(object sender, UserDataApdatedEventArgs args)
        {
            //if (LoggingEnabled)
            //    TraceSource.TraceEvent(TraceEventType.Information, 0, $"User {args.User.LastName} {args.User.PersonalId} was added");
            Communicator?.SendAdd(args);
            Added?.Invoke(sender, args);
        }

        //private bool UserExists(User user)
        //{
        //    return Repository.SearhByPredicate(new Func<User, bool>[]
        //    {
        //        u => u.PersonalId == user.PersonalId
        //    }).Any();
        //}
    }
}
