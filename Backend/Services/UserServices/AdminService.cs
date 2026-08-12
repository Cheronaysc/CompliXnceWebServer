using CompliXnceWebApp.Backend.Dtos;

namespace CompliXnceWebApp.Backend.Services.UserServices
{
    public class AdminService
    {
        //conn string

        public async Task<int> CreateTemplateAsync(CreateTemplateDto dto)
        {
            int a = 0;
            ///Trasnaction, all 20 questions must export together
            //using (var transaction = connection.BeginTransaction())

            ///1. insert the Assesment template title
            //insert into AssesmentTemplate ex exc
            //sql temp
            //dto?//
            //transaction


            ///2. User will insert question from hardcoded bulk seeded questios, 4 of each domain
            //sqlquestion = insert into question exc exc

            //foreach(var q in dto.questions) ---pass new template data and template id

            return a;
        }
    }
}
