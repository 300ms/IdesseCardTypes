using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdesseCardTypes.Pages
{
	public class CardType
	{
		public int Id { get; set; }
		public string Definition { get; set; }
		public Boolean IsVisitable { get; set; }
		public Boolean IsLocationRequired { get; set; }
		public Boolean IsUserAccount { get; set; }

		public CardType(int id, string def, bool isV, bool isL, bool isU)
		{
			Id = id;
			Definition = def;
			IsVisitable = isV;
			IsLocationRequired = isL;
			IsUserAccount = isU;
		}
	}

	public class CardTypes : List<CardType>
	{
		private CardTypes() { }
		private static CardTypes _Instance = null;
		public static CardTypes Instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = new CardTypes();

					for (int i = 0; i < 2; i++)
					{
						_Instance.Add(new CardType(i + 1, $"Taným {i + 1}", true, true, false));
					}
				}
				return _Instance;
			}
		}
	}

	public class CardTypesModel : PageModel
	{
		[BindProperty]
		public int id { get; set; }
		[BindProperty]
		public string definition { get; set; }
		[BindProperty]
		public bool isVisitable { get; set; }
		[BindProperty]
		public bool isLocationRequired { get; set; }
		[BindProperty]
		public bool isUserAccount { get; set; }
		public void OnGet()
		{
		}

		public PartialViewResult OnGetAdd()
		{
			return Partial("_CardTypeAdd", this);
		}

		public RedirectToPageResult OnPostAdd(string definition, bool isVisitable, bool isLocationRequired, bool isUserAccount)
		{
			int idNew = CardTypes.Instance.LastOrDefault().Id + 1;
			CardType ct = new CardType(idNew, definition, isVisitable, isLocationRequired, isUserAccount);
			CardTypes.Instance.Add(ct);
			return new RedirectToPageResult("CardTypes", this);
		}

		public PartialViewResult OnGetEdit()
		{
			return Partial("_CardTypeEdit", this);
		}

		public RedirectToPageResult OnPostEdit(int id, string definition, bool isVisitable, bool isLocationRequired, bool isUserAccount)
		{
			CardType ct = CardTypes.Instance.Find(x => x.Id == id);
			ct.Definition = definition;
			ct.IsVisitable = isVisitable;
			ct.IsLocationRequired = isLocationRequired;
			ct.IsUserAccount = isUserAccount;
			return new RedirectToPageResult("CardTypes", this);
		}
	}
}
