namespace AutoGovernance9Web.Backend.Models
{

    public enum Framework
    {
        COBIT2019,
        ISO38500,
    }
    public enum Domain
    {
        GovernanceAndStrategy,
        RiskManagement,
        SDLCAndChangeManagement,
        EmployeeSatisfaction,
        Ethics,
    }

   
    public class Question
        {
            public int QuestionId { get; set; }
            public string QuestionText { get; set; }
            public int QuestionNumber { get; set; }
            public Framework Framework { get; set; }
            public Domain Domain { get; set; }
    

            public Question()
            {

            }

            public Question(int questionId, string questionText, int questionNumber, Framework framework, Domain domain)
            {
                QuestionId = questionId;
                QuestionText = questionText;
                QuestionNumber = questionNumber;
                Framework = framework;
                Domain = domain;
            }
 


    public static List<Question> GetQuestions()
        {
            return new List<Question>
            {
                //1.governance
                new Question(1, "IT goals and strategic objectives are directly mapped to overall enterprise strategic goals.", 1, Framework.COBIT2019, Domain.GovernanceAndStrategy),
                new Question(2, "Senior executive management actively evaluates and directs IT value delivery on a regular basis.", 2, Framework.COBIT2019, Domain.GovernanceAndStrategy),
                new Question(3, "Stakeholder needs are systematically evaluated to align IT investment decisions with business outcomes.", 3, Framework.COBIT2019, Domain.GovernanceAndStrategy),
                new Question(4, "The governing body directs the preparation and implementation of corporate IT strategies.", 4, Framework.ISO38500, Domain.GovernanceAndStrategy),
                new Question(5, "IT projects and operational activities are routinely monitored against strategic business plans.", 5, Framework.ISO38500, Domain.GovernanceAndStrategy),
                new Question(6, "Roles, decision-making rights, and accountabilities for IT governance are clearly defined and assigned.", 6, Framework.ISO38500, Domain.GovernanceAndStrategy),
                new Question(7, "The organization maintains clear performance indicators (KPIs) to monitor the effectiveness of IT governance processes.", 7, Framework.COBIT2019, Domain.GovernanceAndStrategy),

                //2.risk management
                new Question(8, "An active IT risk management framework is established to identify, assess, and mitigate IT-related risks.", 8, Framework.COBIT2019, Domain.RiskManagement),
                new Question(9, "IT risk appetite and tolerance levels are formally defined and communicated across the business.", 9, Framework.COBIT2019, Domain.RiskManagement),
                new Question(10, "Potential business impacts of cybersecurity threats and system outages are regularly evaluated.", 10, Framework.COBIT2019, Domain.RiskManagement),
                new Question(11, "The governing body ensures IT risk management is integrated into the broader enterprise risk context.", 11, Framework.ISO38500, Domain.RiskManagement),
                new Question(12, "IT operations are regularly audited to verify compliance with risk management policies.", 12, Framework.ISO38500, Domain.RiskManagement),
                new Question(13, "Key risk indicators (KRIs) are continuously tracked and reported to executive leadership.", 13, Framework.COBIT2019, Domain.RiskManagement),
                new Question(14, "Clear procedures exist to ensure business continuity and disaster recovery in the event of major IT disruptions.", 14, Framework.ISO38500, Domain.RiskManagement),

                //3. SDLC
                new Question(15, "Software application development follows a standardized, documented lifecycle methodology.", 15, Framework.COBIT2019, Domain.SDLCAndChangeManagement),
                new Question(16, "System modifications and deployment requests go through a formal approval and testing process prior to production.", 16, Framework.COBIT2019, Domain.SDLCAndChangeManagement),
                new Question(17, "Post-implementation reviews are conducted to verify that delivered systems satisfy functional and security requirements.", 17, Framework.COBIT2019, Domain.SDLCAndChangeManagement),
                new Question(18, "System acquisitions and software purchases are made based on appropriate balance of cost, risk, and strategy.", 18, Framework.ISO38500, Domain.SDLCAndChangeManagement),
                new Question(19, "Project management frameworks are enforced to ensure system delivery stays within scope, budget, and time constraints.", 19, Framework.ISO38500, Domain.SDLCAndChangeManagement),
                new Question(20, "Automated testing and continuous integration mechanisms are used to validate code quality and security.", 20, Framework.COBIT2019, Domain.SDLCAndChangeManagement),
                new Question(21, "Emergency changes are documented, authorized post-facto, and audited to prevent unapproved production code.", 21, Framework.COBIT2019, Domain.SDLCAndChangeManagement),

                //4. employee satisfaction
                new Question(22, "IT staff competencies and skill gaps are regularly assessed and addressed through formal training programs.", 22, Framework.COBIT2019, Domain.EmployeeSatisfaction),
                new Question(23, "Performance metrics for IT staff are aligned with broader IT and operational goals.", 23, Framework.COBIT2019, Domain.EmployeeSatisfaction),
                new Question(24, "Workloads and organizational structures are balanced to promote staff retention and minimize burnout.", 24, Framework.COBIT2019, Domain.EmployeeSatisfaction),
                new Question(25, "The governing body ensures human behavior, user needs, and cultural aspects are respected in IT deployments.", 25, Framework.ISO38500, Domain.EmployeeSatisfaction),
                new Question(26, "Staff training and change management plans are provided to facilitate smooth adoption of new technologies.", 26, Framework.ISO38500, Domain.EmployeeSatisfaction),
                new Question(27, "Roles and responsibilities across IT teams are documented using clear responsibility matrices (e.g., RACI).", 27, Framework.COBIT2019, Domain.EmployeeSatisfaction),
                new Question(28, "User feedback and satisfaction levels regarding IT system usability are systematically tracked and acted upon.", 28, Framework.ISO38500, Domain.EmployeeSatisfaction),

                //5. ethics
                new Question(29, "A formal IT code of conduct and ethics policy is documented and acknowledged by all IT personnel.", 29, Framework.COBIT2019, Domain.Ethics),
                new Question(30, "Processes exist to verify full compliance with regulatory, legal, and contractual requirements governing IT operations.", 30, Framework.COBIT2019, Domain.Ethics),
                new Question(31, "Data privacy, handling, and protection guidelines are enforced throughout the information lifecycle.", 31, Framework.COBIT2019, Domain.Ethics),
                new Question(32, "The organization regularly monitors and conforms to external legislation affecting IT governance.", 32, Framework.ISO38500, Domain.Ethics),
                new Question(33, "Ethical standards regarding data usage, user tracking, and digital assets are explicitly defined and monitored.", 33, Framework.ISO38500, Domain.Ethics),
                new Question(34, "Mechanisms are in place for anonymous reporting of ethical breaches or policy violations within IT operations.", 34, Framework.COBIT2019, Domain.Ethics),
                new Question(35, "Non-compliance incidents and policy breaches are logged, investigated, and remediated systematically.", 35, Framework.ISO38500, Domain.Ethics)
            };
        }
    }
}

