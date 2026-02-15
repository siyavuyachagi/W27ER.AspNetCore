using Domain.Entities.Identity;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Represents a job posting for employment opportunities in Ward 27
    /// </summary>
    public class Job
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Job title
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// Detailed job description
        /// </summary>
        public string Description { get; set; } = default!;

        /// <summary>
        /// Type of work
        /// </summary>
        //public WorkType WorkType { get; set; }

        /// <summary>
        /// Number of positions available
        /// </summary>
        public int PositionsAvailable { get; set; } = 1;

        /// <summary>
        /// Number of positions already filled
        /// </summary>
        public int PositionsFilled { get; set; } = 0;

        /// <summary>
        /// Expected work duration in days
        /// </summary>
        public int? DurationInDays { get; set; }

        /// <summary>
        /// Job start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Job end date (if applicable)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Compensation amount
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CompensationAmount { get; set; }

        /// <summary>
        /// Work location address
        /// </summary>
        public string? WorkLocation { get; set; }

        /// <summary>
        /// Physical address ID for work location
        /// </summary>
        public Guid? WorkLocationAddressId { get; set; }
        [ForeignKey(nameof(WorkLocationAddressId))]
        public PhysicalAddress? WorkLocationAddress { get; set; }

        /// <summary>
        /// Required skills (comma-separated or JSON)
        /// </summary>
        public string? RequiredSkills { get; set; }

        /// <summary>
        /// Minimum education level required
        /// </summary>
        //public EducationLevel? MinimumEducationLevel { get; set; }

        /// <summary>
        /// Minimum years of experience required
        /// </summary>
        public int? MinimumExperienceYears { get; set; }

        /// <summary>
        /// Current status of the job posting
        /// </summary>
        public JobStatusCode StatusCode { get; set; } = JobStatusCode.Draft;

        /// <summary>
        /// User who posted the job (councillor/admin)
        /// </summary>
        public Guid PostedByUserId { get; set; }
        [ForeignKey(nameof(PostedByUserId))]
        public ApplicationUser PostedByUser { get; set; } = default!;

        /// <summary>
        /// Employer/organization offering the job
        /// </summary>
        public Guid? EmployerId { get; set; }
        [ForeignKey(nameof(EmployerId))]
        public Employer? Employer { get; set; }

        /// <summary>
        /// Application deadline
        /// </summary>
        public DateTime? ApplicationDeadline { get; set; }

        /// <summary>
        /// Priority level for matching (higher = more urgent)
        /// </summary>
        public int Priority { get; set; } = 0;

        /// <summary>
        /// Is this job urgent?
        /// </summary>
        public bool IsUrgent { get; set; } = false;

        /// <summary>
        /// Is this job featured/highlighted?
        /// </summary>
        public bool IsFeatured { get; set; } = false;

        /// <summary>
        /// Number of views
        /// </summary>
        public int ViewCount { get; set; } = 0;

        /// <summary>
        /// Number of applications received
        /// </summary>
        public int ApplicationCount => JobApplications.Count;

        /// <summary>
        /// Contact person name
        /// </summary>
        public string? ContactPersonName { get; set; }

        /// <summary>
        /// Contact email
        /// </summary>
        public string? ContactEmail { get; set; }

        /// <summary>
        /// Contact phone number
        /// </summary>
        public string? ContactPhone { get; set; }

        /// <summary>
        /// Additional requirements or notes
        /// </summary>
        public string? AdditionalNotes { get; set; }

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        // Navigation properties
        public ICollection<JobAssignment> JobAssignments { get; set; } = new List<JobAssignment>();
        public ICollection<JobApplication> JobApplications { get; set; } = [];

        // Computed properties

        /// <summary>
        /// Number of remaining positions
        /// </summary>
        public int RemainingPositions => PositionsAvailable - JobAssignments.Count;

        /// <summary>
        /// Is the job currently active and accepting applications?
        /// </summary>
        public bool IsActive => StatusCode == JobStatusCode.Published && !IsDeleted;

        /// <summary>
        /// Days until application deadline
        /// </summary>
        public int? DaysUntilDeadline
        {
            get
            {
                if (!ApplicationDeadline.HasValue) return null;
                var days = (ApplicationDeadline.Value - DateTime.UtcNow).Days;
                return days >= 0 ? days : 0;
            }
        }

        /// <summary>
        /// Duration text (e.g., "3 months", "1 week")
        /// </summary>
        public string? DurationFormatted
        {
            get
            {
                if (!DurationInDays.HasValue) return null;

                if (DurationInDays.Value < 7)
                    return $"{DurationInDays.Value} day{(DurationInDays.Value != 1 ? "s" : "")}";

                if (DurationInDays.Value < 30)
                {
                    var weeks = DurationInDays.Value / 7;
                    return $"{weeks} week{(weeks != 1 ? "s" : "")}";
                }

                var months = DurationInDays.Value / 30;
                return $"{months} month{(months != 1 ? "s" : "")}";
            }
        }

        /// <summary>
        /// Formatted compensation (e.g., "R5,000.00")
        /// </summary>
        public string? CompensationFormatted => CompensationAmount.HasValue
            ? $"R{CompensationAmount.Value:N2}"
            : null;
    }
}
