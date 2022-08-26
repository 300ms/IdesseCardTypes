using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdesseCardTypes.Pages;

public class CardType
{
    public CardType(int id, string def, bool isV, bool isL, bool isU)
    {
        Id = id;
        Definition = def;
        IsVisitable = isV;
        IsLocationRequired = isL;
        IsUserAccount = isU;
    }

    public int Id { get; set; }
    public string Definition { get; set; }
    public bool IsVisitable { get; set; }
    public bool IsLocationRequired { get; set; }
    public bool IsUserAccount { get; set; }
}

public class CardTypes : List<CardType>
{
    private static CardTypes _Instance;

    private CardTypes()
    {
    }

    public static CardTypes Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new CardTypes();

                for (var i = 0; i < 80; i++) _Instance.Add(new CardType(i + 1, $"Taným {i + 1}", true, true, false));
            }

            return _Instance;
        }
    }
}

public class CardTypesModel : PageModel
{
    public void OnGet()
    {
    }

    public PartialViewResult OnGetAdd()
    {
        CardType ct = new CardType(0, string.Empty, false, false, false);
        return Partial("_CardTypeAddEdit", ct);
    }

    public PartialViewResult OnPostAdd(int id, string definition, bool isVisitable, bool isLocationRequired,
        bool isUserAccount)
    {
        var check = CardTypes.Instance.Where(x => x.Id != id && x.Definition == definition).FirstOrDefault();
        if (check == null && !String.IsNullOrEmpty(definition))
        {
            if (id == 0)
            {
                id = CardTypes.Instance.LastOrDefault().Id + 1;
                CardType ct = new CardType(id, definition, isVisitable, isLocationRequired, isUserAccount);
                CardTypes.Instance.Add(ct);
                Response.ContentType = "text/vnd.turbo-stream.html";
                return Partial("_CardTypeAdd", ct);
            }
            else
            {
                CardType ct = CardTypes.Instance.Where(x => x.Id == id).FirstOrDefault();
                ct.Definition = definition;
                ct.IsVisitable = isVisitable;
                ct.IsLocationRequired = isLocationRequired;
                ct.IsUserAccount = isUserAccount;
                Response.ContentType = "text/vnd.turbo-stream.html";
                return Partial("_CardTypeEdit", ct);
            }
        }
        else
        {
            if (check != null)
            {
                CardType ct = new CardType(0, string.Empty, false, false, false);
                return Partial("_DefinitionAlreadyExists", ct);
            }
            else
            {
                CardType ct = new CardType(0, string.Empty, false, false, false);
                return Partial("_DefinitionEmpty", ct);
            }
        }
    }

    public PartialViewResult OnGetEdit(int id, string definition, bool isVisitable, bool isLocationRequired,
        bool isUserAccount)
    {
        CardType ct = CardTypes.Instance.Where(x => x.Id == id).FirstOrDefault();
        return Partial("_CardTypeAddEdit", ct);
    }

    public RedirectToPageResult OnPostEdit(int id, string definition, bool isVisitable, bool isLocationRequired,
        bool isUserAccount)
    {
        CardType ct = CardTypes.Instance.Where(x => x.Id == id).FirstOrDefault();
        ct.Definition = definition;
        ct.IsVisitable = isVisitable;
        ct.IsLocationRequired = isLocationRequired;
        ct.IsUserAccount = isUserAccount;
        return new RedirectToPageResult("CardTypes", this);
    }

    public PartialViewResult OnPostDelete(int id)
    {
        CardType ct = CardTypes.Instance.Where(x => x.Id == id).FirstOrDefault();
        CardTypes.Instance.Remove(ct);

        Response.ContentType = "text/vnd.turbo-stream.html";
        return Partial("_CardTypeDelete", ct);
    }
}