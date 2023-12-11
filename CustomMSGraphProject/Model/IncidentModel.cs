namespace CustomMSGraphProject.Model;
public class IncidentModel
{
    public string Incident_id { get; set; }
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public string? Assigned_to { get; set; }
    public DateTime Created_at { get; set; }
    public DateTime Updated_at { get; set; }
    public List<CommentModel>? Comments { get; set; }
    public List<AttachmentModel>? Attachments { get; set; }
    public CustomFieldsModel? Custom_fields { get; set; }
}