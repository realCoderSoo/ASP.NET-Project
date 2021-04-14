# ASP.NET-Project
The project creates a School Community Web App.

Views
Students/Index
You need to add options for the user to:
Select each student and see the communities s/he is a member of
Think whether you need one of the ViewModels in the hints section. My wild guess is that you need it!
Edit the list of community memberships for each student, which happens in Students/EditMemberships
Students/EditMemberships
You need to enable the user to register/unregister users to communities
Hint: Clicking Register/Unregister should invoke different actions in StudentsController, called Addmembership and RemoveMembership respectively. These actions should get studentId and communityId as parameters. Well, after all, these form the composite key for the CommunityMembership table. So, in total, you need one view, but three controller actions handling it. You can combine it into one controller action, but why complicate it when there is no need for!?
Hmmm….Looks like a perfect candidate for a ViewModel. Don’t you think so?
The way I have it, the communities that the student is a member of go first, followed by the rest of communities sorted alphabetically. You can change this sorting order, but there must be sorting logic, and your demo should specify how you decided to go with sorting.
Hint: Sorting can be either in your query or using Linq after you extracted results into lists. It is up to you to choose the easier solution (the latter one)    
Communities/Index
You need to add a link “Ads” that will direct to a page that has a list of images that the community posted on their forum. (we call them ads)
Where do you need to make the change? Controller? View? I think it is more of a View change.
Advertisements/Index/{id}
Notice that, for simplicity, we do not have Advertisements/Index; I mean Index not followed by {id}. You need to pass the id of the community you want to see the ads for. This view will show all the ads for community with id in the path ({id}).
You should be able to delete an ad and that will take you to Advertisements/Delete/{ad id} for verification and completing the delete action. Needless to say that you need a delete view here as well.
Let us make it simple, so we do not have to deal with DB constraints. We already have a lot to take care of. For simplicity, let us make sure that we delete the community only if all its ads are deleted. So, we will not allow deletion if the community already has ads.
Advertisements/Create/{id}
The user should be able to upload an ad for a community whose id is in the path ({id}).
Do you wonder how to path additional data along with the file you upload? Maybe a quick look into hints and googling hidden input tags would help. 
Hints
ViewModels are your best friend. Use them as much as possible. You will probably need another 4 of them for this assignment (if you choose to use ViewModels). Remember that you can put anything in ViewModels, they are just simple classes. For instance, all the followings are valid ViewModels:
public class FileInputViewModel
{
public string CommunityId { get; set; }
public string CommunityTitle { get; set; }
public IFormFile File { get; set; }
}

public class AdsViewModel
{
public Community Community { get; set; }
public IEnumerable<Advertisement> Advertisements { get; set; }
}

    public class StudentMembershipViewModel
    {
        public Student Student { get; set; }
        public IEnumerable<StudentViewModel> Memberships { get; set; }
    }

    public class CommunityMembersipViewModel
    {
        public string CommunityId { get; set; }
        public string Title { get; set; }
        public bool IsMember { get; set; }
    }
