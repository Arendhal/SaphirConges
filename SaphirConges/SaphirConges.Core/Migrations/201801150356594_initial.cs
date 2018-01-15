namespace HolidayApp.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CallLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        Action = c.String(),
                        Reminder = c.DateTime(precision: 7, storeType: "datetime2"),
                        Status = c.String(),
                        WelcomeEmail = c.String(),
                        Client_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ContactNumber = c.String(nullable: false),
                        Mobile = c.String(),
                        EmailId = c.String(),
                        Address = c.String(),
                        Status = c.String(),
                        ResellerClientKey = c.Int(nullable: false),
                        CreatedTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        UpdatedTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        ClientIndustry_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClientIndustries", t => t.ClientIndustry_Id)
                .Index(t => t.ClientIndustry_Id);
            
            CreateTable(
                "dbo.ClientIndustries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DomainOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResellerKey = c.Int(nullable: false),
                        Url = c.String(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Registered = c.DateTime(nullable: false),
                        Expiry = c.DateTime(nullable: false),
                        Status = c.String(),
                        Client_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "dbo.Meetings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Datetime = c.DateTime(nullable: false),
                        Outcome = c.String(),
                        Remarks = c.String(),
                        CallLog_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CallLogs", t => t.CallLog_Id)
                .Index(t => t.CallLog_Id);
            
            CreateTable(
                "dbo.Conges",
                c => new
                    {
                        CongesID = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        NoOfDays = c.Single(nullable: false),
                        HalfDay = c.String(),
                        Statut = c.String(),
                        BookingDate = c.DateTime(nullable: false),
                        BookedBy = c.String(),
                        TypeConges = c.String(nullable: false),
                        CongesDescription = c.String(),
                        Employe_EmployeeId = c.Int(),
                    })
                .PrimaryKey(t => t.CongesID)
                .ForeignKey("dbo.Employees", t => t.Employe_EmployeeId)
                .Index(t => t.Employe_EmployeeId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        Gender = c.Int(nullable: false),
                        HireDate = c.DateTime(nullable: false),
                        TerminationDate = c.DateTime(),
                        Address = c.String(),
                        ContactNumber = c.Long(nullable: false),
                        PersonalEmail = c.String(),
                        Username = c.String(),
                        EmployeeType = c.Int(nullable: false),
                        JobTitle = c.String(),
                    })
                .PrimaryKey(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Budget = c.Single(nullable: false),
                        StartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Cost = c.Double(),
                        Balance = c.Double(),
                        Source = c.String(),
                        ConversionDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        HarvestInvoiceId = c.Long(),
                        Client_Id = c.Int(),
                        Domain_Id = c.Int(),
                        HostingPackage_Id = c.Int(),
                        SalesPerson_EmployeeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id)
                .ForeignKey("dbo.DomainOrders", t => t.Domain_Id)
                .ForeignKey("dbo.Hostings", t => t.HostingPackage_Id)
                .ForeignKey("dbo.Employees", t => t.SalesPerson_EmployeeId)
                .Index(t => t.Client_Id)
                .Index(t => t.Domain_Id)
                .Index(t => t.HostingPackage_Id)
                .Index(t => t.SalesPerson_EmployeeId);
            
            CreateTable(
                "dbo.Hostings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FromDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ToDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Cost = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CongesDescriptions",
                c => new
                    {
                        CongesDescriptionID = c.Int(nullable: false, identity: true),
                        TypeConge = c.String(nullable: false),
                        CongesColor = c.String(nullable: false),
                        TypePour = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CongesDescriptionID);
            
            CreateTable(
                "dbo.CongesGenerals",
                c => new
                    {
                        CongesGeneralID = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false),
                        Description = c.String(),
                        Type = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        NoOfDays = c.Single(nullable: false),
                        Frequency = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CongesGeneralID);
            
            CreateTable(
                "dbo.Attendances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BusinessDateKey = c.Int(nullable: false),
                        TimeIn = c.Time(precision: 7),
                        TimeOut = c.Time(precision: 7),
                        BusinessDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IpAddress = c.String(),
                        Employee_EmployeeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Employee_EmployeeId)
                .Index(t => t.Employee_EmployeeId);
            
            CreateTable(
                "dbo.EmployeeSalaries",
                c => new
                    {
                        EmployeeSalaryId = c.Int(nullable: false, identity: true),
                        Salary = c.Long(nullable: false),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(),
                        Employee_EmployeeId = c.Int(),
                    })
                .PrimaryKey(t => t.EmployeeSalaryId)
                .ForeignKey("dbo.Employees", t => t.Employee_EmployeeId)
                .Index(t => t.Employee_EmployeeId);
            
            CreateTable(
                "dbo.EmployeQuotas",
                c => new
                    {
                        EmployeQuotaID = c.Int(nullable: false, identity: true),
                        EmployeID = c.Int(nullable: false),
                        PaidQuota = c.Double(nullable: false),
                        NonPaidQuota = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeQuotaID)
                .ForeignKey("dbo.Employees", t => t.EmployeID, cascadeDelete: true)
                .Index(t => t.EmployeID);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaymentMethod = c.Int(nullable: false),
                        ChequeNumber = c.Int(),
                        PaymentDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Amount = c.Double(),
                        HarvestPaymentId = c.Long(),
                        Project_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.Portfolios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Url = c.String(nullable: false),
                        Status = c.Boolean(nullable: false),
                        ClientIndustry_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClientIndustries", t => t.ClientIndustry_Id)
                .Index(t => t.ClientIndustry_Id);
            
            CreateTable(
                "dbo.ProjectLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeLog = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        UpdatedBy = c.String(),
                        DailyLog = c.Time(precision: 7),
                        UpdatedTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IpAddress = c.String(),
                        Description = c.String(),
                        AsanaUrl = c.String(),
                        AsanaTaskId = c.Long(nullable: false),
                        AsanaTaskName = c.String(),
                        Participant_EmployeeId = c.Int(),
                        Project_Id = c.Int(),
                        Tag_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Participant_EmployeeId)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .ForeignKey("dbo.Tags", t => t.Tag_Id)
                .Index(t => t.Participant_EmployeeId)
                .Index(t => t.Project_Id)
                .Index(t => t.Tag_Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsBillable = c.Boolean(nullable: false),
                        RequiresDescription = c.Boolean(nullable: false),
                        RequiresAsanaUrl = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Watermarks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Source = c.String(),
                        LowWatermark = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectEmployee",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.EmployeeId })
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectLogs", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.ProjectLogs", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.ProjectLogs", "Participant_EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Portfolios", "ClientIndustry_Id", "dbo.ClientIndustries");
            DropForeignKey("dbo.Payments", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.EmployeQuotas", "EmployeID", "dbo.Employees");
            DropForeignKey("dbo.EmployeeSalaries", "Employee_EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Attendances", "Employee_EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Conges", "Employe_EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Projects", "SalesPerson_EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.ProjectEmployee", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.ProjectEmployee", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Projects", "HostingPackage_Id", "dbo.Hostings");
            DropForeignKey("dbo.Projects", "Domain_Id", "dbo.DomainOrders");
            DropForeignKey("dbo.Projects", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.Meetings", "CallLog_Id", "dbo.CallLogs");
            DropForeignKey("dbo.DomainOrders", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.Clients", "ClientIndustry_Id", "dbo.ClientIndustries");
            DropForeignKey("dbo.CallLogs", "Client_Id", "dbo.Clients");
            DropIndex("dbo.ProjectEmployee", new[] { "EmployeeId" });
            DropIndex("dbo.ProjectEmployee", new[] { "ProjectId" });
            DropIndex("dbo.ProjectLogs", new[] { "Tag_Id" });
            DropIndex("dbo.ProjectLogs", new[] { "Project_Id" });
            DropIndex("dbo.ProjectLogs", new[] { "Participant_EmployeeId" });
            DropIndex("dbo.Portfolios", new[] { "ClientIndustry_Id" });
            DropIndex("dbo.Payments", new[] { "Project_Id" });
            DropIndex("dbo.EmployeQuotas", new[] { "EmployeID" });
            DropIndex("dbo.EmployeeSalaries", new[] { "Employee_EmployeeId" });
            DropIndex("dbo.Attendances", new[] { "Employee_EmployeeId" });
            DropIndex("dbo.Projects", new[] { "SalesPerson_EmployeeId" });
            DropIndex("dbo.Projects", new[] { "HostingPackage_Id" });
            DropIndex("dbo.Projects", new[] { "Domain_Id" });
            DropIndex("dbo.Projects", new[] { "Client_Id" });
            DropIndex("dbo.Conges", new[] { "Employe_EmployeeId" });
            DropIndex("dbo.Meetings", new[] { "CallLog_Id" });
            DropIndex("dbo.DomainOrders", new[] { "Client_Id" });
            DropIndex("dbo.Clients", new[] { "ClientIndustry_Id" });
            DropIndex("dbo.CallLogs", new[] { "Client_Id" });
            DropTable("dbo.ProjectEmployee");
            DropTable("dbo.Watermarks");
            DropTable("dbo.Tags");
            DropTable("dbo.ProjectLogs");
            DropTable("dbo.Portfolios");
            DropTable("dbo.Payments");
            DropTable("dbo.EmployeQuotas");
            DropTable("dbo.EmployeeSalaries");
            DropTable("dbo.Attendances");
            DropTable("dbo.CongesGenerals");
            DropTable("dbo.CongesDescriptions");
            DropTable("dbo.Hostings");
            DropTable("dbo.Projects");
            DropTable("dbo.Employees");
            DropTable("dbo.Conges");
            DropTable("dbo.Meetings");
            DropTable("dbo.DomainOrders");
            DropTable("dbo.ClientIndustries");
            DropTable("dbo.Clients");
            DropTable("dbo.CallLogs");
        }
    }
}
