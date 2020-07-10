using System;

using Windows.ApplicationModel.Activation;

using GPB.ViewModels;

namespace GPB.Services
{
    #region ActivationInfo
    public class ActivationInfo
    {
        static public ActivationInfo CreateDefault() => Create<DashboardViewModel>();

        static public ActivationInfo Create<TViewModel>(object entryArgs = null) where TViewModel : ViewModelBase
        {
            return new ActivationInfo
            {
                EntryViewModel = typeof(TViewModel),
                EntryArgs = entryArgs
            };
        }

        public Type EntryViewModel { get; set; }
        public object EntryArgs { get; set; }
    }
    #endregion

    static public class ActivationService
    {
        static public ActivationInfo GetActivationInfo(IActivatedEventArgs args)
        {
            switch (args.Kind)
            {
                case ActivationKind.Protocol:
                    return GetProtocolActivationInfo(args as ProtocolActivatedEventArgs);

                case ActivationKind.Launch:
                default:
                    return ActivationInfo.CreateDefault();
            }
        }

        private static ActivationInfo GetProtocolActivationInfo(ProtocolActivatedEventArgs args)
        {
            //if (args != null)
            //{
            //    switch (args.Uri.AbsolutePath.ToLowerInvariant())
            //    {
            //        case "student":
            //        case "students":
            //            long studentID = args.Uri.GetInt64Parameter("id");
            //            if (studentID > 0)
            //            {
            //                return ActivationInfo.Create<StudentDetailsViewModel>(new StudentDetailsArgs { StudentID = studentID });
            //            }
            //            return ActivationInfo.Create<StudentsViewModel>(new StudentListArgs());
            //        case "teacher":
            //        case "teachers":
            //            long teacherID = args.Uri.GetInt64Parameter("id");
            //            if (teacherID > 0)
            //            {
            //                return ActivationInfo.Create<TeacherDetailsViewModel>(new TeacherDetailsArgs { TeacherID = teacherID });
            //            }
            //            return ActivationInfo.Create<TeachersViewModel>(new TeacherListArgs());
            //        case "course":
            //        case "courses":
            //            string courseID = args.Uri.GetParameter("id");
            //            if (courseID != null)
            //            {
            //                return ActivationInfo.Create<CourseDetailsViewModel>(new CourseDetailsArgs { CourseID = courseID });
            //            }
            //            return ActivationInfo.Create<CoursesViewModel>(new CourseListArgs());
            //    }
            //}
            return ActivationInfo.CreateDefault();
        }
    }
}
