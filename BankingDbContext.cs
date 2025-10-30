using Microsoft.EntityFrameworkCore;
using Banking_CapStone.Model;
using System.Reflection.Emit;

namespace Banking_CapStone.Data
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserBase> Users { get; set; }
        public DbSet<SuperAdmin> SuperAdmins { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankUser> BankUsers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountStatus> AccountStatuses { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<PaymentStatus> PaymentStatuses { get; set; }
        public DbSet<ProofType> ProofTypes { get; set; }
        public DbSet<Query> Queries { get; set; }
        public DbSet<SalaryDisbursement> SalaryDisbursements { get; set; }
        public DbSet<SalaryDisbursementDetails> SalaryDisbursementDetails { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<UserBase>()
            .HasDiscriminator<string>("UserType")
            .HasValue<SuperAdmin>("SuperAdmin")
            .HasValue<BankUser>("BankUser")
            .HasValue<Client>("Client");

            // ========== USer ROLE RELATIONSHIP ==========
            modelBuilder.Entity<UserBase>()
                .HasOne(u => u.UserRole)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== SUPERADMIN → BANK ==========
            modelBuilder.Entity<SuperAdmin>()
                .HasMany(sa => sa.Banks)
                .WithOne(b => b.SuperAdmin)
                .HasForeignKey(b => b.SuperAdminId)
                .OnDelete(DeleteBehavior.Restrict);

    //Bank Part start 

            // ========== BANK → BANKUSER ==========
            modelBuilder.Entity<Bank>()
                .HasMany(b => b.BankUsers)
                .WithOne(bu => bu.Bank)
                .HasForeignKey(bu => bu.BankId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== BANK → CLIENT ==========
            modelBuilder.Entity<Bank>()
                .HasMany(b => b.Clients)
                .WithOne(c => c.Bank)
                .HasForeignKey(c => c.BankId)
                .OnDelete(DeleteBehavior.Cascade);

            // Bank -> Account 
            modelBuilder.Entity<Bank>()
                .HasMany(b => b.Accounts)
                .WithOne(a => a.Bank)
                .HasForeignKey(a => a.BankId)
                .OnDelete(DeleteBehavior.Restrict);

            //Client => Account
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Accounts)
                .WithOne(a => a.Client)
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            //account => accountstatus 
            modelBuilder.Entity<Account>()
                .HasOne(a=>a.AccountStatus)
                .WithMany(c => c.Accounts)
                .HasForeignKey(a => a.AccountStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // account - > transaction 
            modelBuilder.Entity<Account>()
               .HasMany(a => a.Transactions)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

  //Bank Part end

    //Client start

            // ========== CLIENT → EMPLOYEE ==========
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Employees)
                .WithOne(e => e.Client)
                .HasForeignKey(e => e.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            //client -> beneficiary 
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Beneficiaries)
                .WithOne(b => b.Client)
                .HasForeignKey(b => b.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========== CLIENT → PAYMENT ==========
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Payments)
                .WithOne(p => p.Client)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========== CLIENT → TRANSACTION ==========
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Transactions)
                .WithOne(t => t.Client)
                .HasForeignKey(t => t.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========== CLIENT → DOCUMENT ==========
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Documents)
                .WithOne(d => d.Client)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // CLIENT → SALARYDISBURSEMENT 
            modelBuilder.Entity<Client>()
                .HasMany(c => c.SalaryDisbursements)
                .WithOne(sd => sd.Client)
                .HasForeignKey(sd => sd.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            //client end


            // ============= Beneficiary = > payment
            modelBuilder.Entity<Beneficiary>()
            .HasMany(b => b.Payments)
            .WithOne(p => p.Beneficiary)
            .HasForeignKey(p => p.BeneficiaryId)
            .OnDelete(DeleteBehavior.Restrict);

            // ========== EMPLOYEE → PAYMENT ==========
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Payments)
                .WithOne(p => p.Employee)
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // EMPLOYEE → SALARYDISBURSEMENTDETAILS
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.SalaryDisbursementDetails)
                .WithOne(sdd => sdd.Employee)
                .HasForeignKey(sdd => sdd.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);


            // ========== BANKUSER → PAYMENT ==========
            modelBuilder.Entity<BankUser>()
                .HasMany(bu => bu.Payments)
                .WithOne(p => p.BankUser)
                .HasForeignKey(p => p.BankUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== BANKUSER → TRANSACTION ==========
            modelBuilder.Entity<BankUser>()
                .HasMany(bu => bu.Transactions)
                .WithOne(t => t.BankUser)
                .HasForeignKey(t => t.BankUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== BANKUSER → SALARYDISBURSEMENT ==========
            modelBuilder.Entity<BankUser>()
                .HasMany(bu => bu.ApprovedSalaryDisbursement)
                .WithOne(sd => sd.ApprovedBy)
                .HasForeignKey(sd => sd.ApprovedByBankUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========== PAYMENTSTATUS → PAYMENT ===============//
            modelBuilder.Entity<PaymentStatus>()
                .HasMany(ps => ps.Payments)
                .WithOne(p => p.PaymentStatus)
                .HasForeignKey(p => p.PaymentStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== PAYMENTSTATUS → SALARYDISBURSEMENT (NEW) ==========
            modelBuilder.Entity<PaymentStatus>()
                .HasMany(ps => ps.SalaryDisbursements)
                .WithOne(sd => sd.DisbursementStatus)
                .HasForeignKey(sd => sd.DisbursementStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            //PROOFTYPE → DOCUMENT 
            modelBuilder.Entity<ProofType>()
                .HasMany(pt => pt.Documents)
                .WithOne(d =>  d.ProofType)
                .HasForeignKey(d => d.ProofTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // SALARYDISBURSEMENT → SALARYDISBURSEMENTDETAILS 
            modelBuilder.Entity<SalaryDisbursement>()
                .HasMany(sd => sd.DisbursementDetials)
                .WithOne(sdd => sdd.SalaryDisbursement)
                .HasForeignKey(sdd => sdd.SalaryDisbursementId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========== SALARYDISBURSEMENT → TRANSACTION (NEW) ==========
            modelBuilder.Entity<SalaryDisbursement>()
                .HasMany(sd => sd.Transactions)
                .WithOne(t => t.SalaryDisbursement)
                .HasForeignKey(t => t.SalaryDisbursementId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== SALARYDISBURSEMENTDETAILS → TRANSACTION (NEW) ==========
            modelBuilder.Entity<SalaryDisbursementDetails>()
                .HasOne(sdd => sdd.Transaction)
                .WithOne(t => t.SalaryDisbursementDetail)
                .HasForeignKey<SalaryDisbursementDetails>(sdd => sdd.TransactionId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== TRANSACTIONTYPE → TRANSACTION (NEW) ==========
            modelBuilder.Entity<TransactionType>()
                .HasMany(tt => tt.Transactions)
                .WithOne(t => t.TransactionType)
                .HasForeignKey(t => t.TransactionTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== PAYMENT → TRANSACTION ==========
            modelBuilder.Entity<Payment>()
                .HasMany(p => p.Transactions)
                .WithOne(t => t.Payment)
                .HasForeignKey(t => t.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== USER → AUDIT LOG ==========
            modelBuilder.Entity<UserBase>()
                .HasMany(u => u.AuditLogs)
                .WithOne(al => al.User)
                .HasForeignKey(al => al.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========== QUERY → USER  ==========
            modelBuilder.Entity<Query>()
                .HasOne(q => q.RespondedByUser)
                .WithMany()
                .HasForeignKey(q => q.RespondedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== UNIQUE CONSTRAINTS ==========
            modelBuilder.Entity<UserBase>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<UserBase>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.AccountNumber)
                .IsUnique();

            modelBuilder.Entity<Bank>()
                .HasIndex(b => b.IFSCCode)
                .IsUnique();

            // ========== DECIMAL PRECISION ==========
            modelBuilder.Entity<Client>()
                .Property(c => c.AccountBalance)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Account>()
                .Property(a => a.Balance)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<SalaryDisbursement>()
                .Property(sd => sd.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<SalaryDisbursementDetails>()
                .Property(sdd => sdd.Amount)
                .HasColumnType("decimal(18,2)");

            // inital balance of everyone 

            modelBuilder.Entity<Client>()
    .Property(c => c.AccountBalance)
    .HasDefaultValue(0);

            modelBuilder.Entity<Account>()
                .Property(a => a.Balance)
                .HasDefaultValue(0);

            modelBuilder.Entity<UserBase>()
                .Property(u => u.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Employee>()
                .Property(e => e.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Beneficiary>()
                .Property(b => b.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Query>()
                .Property(q => q.IsResolved)
                .HasDefaultValue(false);


            //assgining value to each role 

            // Seed UserRoles
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { RoleId = 1, Role = Role.SUPER_ADMIN, Description = "Super Administrator" },
                new UserRole { RoleId = 2, Role = Role.BANK_USER, Description = "Bank Employee" },
                new UserRole { RoleId = 3, Role = Role.CLIENT_USER, Description = "Corporate Client" }
            );

            // Seed AccountTypes
            modelBuilder.Entity<AccountType>().HasData(
                new AccountType { TypeId = 1, Type = AccType.SAVING },
                new AccountType { TypeId = 2, Type = AccType.CURRENT },
                new AccountType { TypeId = 3, Type = AccType.SALARY }
            );

            // Seed AccountStatuses
            modelBuilder.Entity<AccountStatus>().HasData(
                new AccountStatus { StatusId = 1, Status = AccStatus.ACTIVE },
                new AccountStatus { StatusId = 2, Status = AccStatus.INACTIVE },
                new AccountStatus { StatusId = 3, Status = AccStatus.CLOSED }
            );

            // Seed PaymentStatuses
            modelBuilder.Entity<PaymentStatus>().HasData(
                new PaymentStatus { StatusId = 1, Status = PayStatus.APPROVED },
                new PaymentStatus { StatusId = 2, Status = PayStatus.DECLINED },
                new PaymentStatus { StatusId = 3, Status = PayStatus.PENDING }
            );

            // Seed TransactionTypes
            modelBuilder.Entity<TransactionType>().HasData(
                new TransactionType { TypeId = 1, Type = TxnType.CREDIT, Description = "Money In" },
                new TransactionType { TypeId = 2, Type = TxnType.DEBIT, Description = "Money Out" }
            );

            // Seed ProofTypes
            modelBuilder.Entity<ProofType>().HasData(
                new ProofType { TypeId = 1, Type = DocProofType.IDENTITY_PROOF, Description = "Aadhaar/Passport/Voter ID" },
                new ProofType { TypeId = 2, Type = DocProofType.ADDRESS_PROOF, Description = "Utility Bill/Rent Agreement" },
                new ProofType { TypeId = 3, Type = DocProofType.DATE_OF_BIRTH_PROOF, Description = "Birth Certificate" },
                new ProofType { TypeId = 4, Type = DocProofType.PHOTOGRAPH, Description = "Passport Photo" },
                new ProofType { TypeId = 5, Type = DocProofType.PAN_CARD, Description = "PAN Card" },
                new ProofType { TypeId = 6, Type = DocProofType.SIGNATURE, Description = "Signature Specimen" },
                new ProofType { TypeId = 7, Type = DocProofType.BANK_STATEMENT, Description = "Bank Statement" },
                new ProofType { TypeId = 8, Type = DocProofType.BUSINESS_PROOF, Description = "Company Registration" },
                new ProofType { TypeId = 9, Type = DocProofType.OTHER, Description = "Other Documents" }
            );

        }
    }
}
