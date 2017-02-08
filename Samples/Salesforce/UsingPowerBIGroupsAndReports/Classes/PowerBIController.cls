public class PowerBIController extends OAuthController {
    @TestVisible private static String APPLICATION_NAME = 'PowerBI';

    public PowerBIController () {
        this.application_name = APPLICATION_NAME;
    }
    
    public String getValidateResult()
    {
        return validateResult;
    }
    
    public String debug {get;set;}
    
    public String selectedReport{ get; set; }    
    public List<SelectOption> availableReports {get;set;}
    
    public void getReports()
    {
    	availableReports = new List<SelectOption>();
    	List<Object> objs;
    	
    	if(this.PBIaccess_token != '' && this.selectedGroup != null)
		{
			Http h = new Http();
			String selectedGroupId;

			for(SelectOption s : availableGroups)
			{
				debug = s.getLabel()  + ' ' + selectedGroup ;
				if(selectedGroup == s.getValue())
				{
					selectedGroupId = s.getValue();					
				}
			}
			
			String reportsUrl = 'https://api.powerbi.com/v1.0/myorg/groups/'+selectedGroupId+'/reports';
			HttpRequest req = new HttpRequest();
        	req.setEndpoint(reportsUrl);
        	req.setMethod('GET');
	        req.setHeader('Content-Type', 'application/json');
	        req.setHeader('Authorization','Bearer ' + this.PBIaccess_token);
        	
        	HttpResponse res = h.send(req);
        	if(res.getStatusCode() == 200)
        	{
        		Map<String, Object> tmpResult = (Map<String,Object>)JSON.deserializeUntyped(res.getBody());
        		objs = (List<Object>)tmpResult.get('value');
				for(Object ob : objs)
				{
					PBI_Report report = (PBI_Report)(JSON.deserialize(JSON.serialize(ob), PBI_Report.class));	
					availableReports.add(new SelectOption(report.id ,report.name));
				}
  	    	}
		}
    }  
    
    public String selectedGroup{ get; set; }    
    public List<SelectOption> availableGroups {get;set;}

	public void getGroups()
	{
		availableGroups = new List<SelectOption>();
		List<Object> objs;
		if(this.PBIaccess_token != '')
		{
			Http h = new Http();
			String groupsUrl = 'https://api.powerbi.com/v1.0/myorg/groups';
			HttpRequest req = new HttpRequest();
        	req.setEndpoint(groupsUrl);
        	req.setMethod('GET');
	        req.setHeader('Content-Type', 'application/json');
	        req.setHeader('Authorization','Bearer ' + this.PBIaccess_token);
        	
        	HttpResponse res = h.send(req);
        	if(res.getStatusCode() == 200)
        	{
        		Map<String, Object> tmpResult = (Map<String,Object>)JSON.deserializeUntyped(res.getBody());
        		objs = (List<Object>)tmpResult.get('value');
        		
				for(Object ob : objs)
				{
					PBI_Group pbiGroup = (PBI_Group)(JSON.deserialize(JSON.serialize(ob), PBI_Group.class));	
					availableGroups.add(new SelectOption(pbiGroup.id ,pbiGroup.name));
				}
        	}
		}
		
	}
    
    /**
    * Validates the callback code and generates the access and refresh tokens
    *
    * @return null to refresh the page
    */
    public PageReference redirectOnCallback() {
        return super.redirectOnCallback(null);
    }
    
    public PageReference refreshAccessToken() {
        return super.refreshAccessToken(ApexPages.currentPage());
    }
               
    public class PBI_Group {
    	public String id {get;set;}
    	public Boolean isReadOnly {get;set;}
    	public String name{get;set;}	
    }
    
    public class PBI_Report{
    	public String id {get;set;}
    	public String name {get;set;}
    	public string webUrl {get;set;}
    	public String embedUrl {get;set;}
    }
}