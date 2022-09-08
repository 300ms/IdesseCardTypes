using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.SignalR;
using IdesseCardTypes.Hubs;
using IdesseCardTypes.Services;

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
	private readonly IHubContext<AppHub> _Hub;
	private readonly IRazorPartialToStringRenderer _Renderer;

	public CardTypesModel(IHubContext<AppHub> hub, IRazorPartialToStringRenderer renderer)
	{
		_Hub = hub;
		_Renderer = renderer;
	}
	public void OnGet()
	{
	}

	public PartialViewResult OnGetAdd()
	{
		CardType ct = new CardType(0, string.Empty, false, false, false);
		return Partial("_CardTypeAddEdit", ct);
	}

	public async Task<IActionResult> OnPostAdd(int id, string definition, bool isVisitable, bool isLocationRequired,
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

				var renderedViewStr = await _Renderer.RenderPartialToStringAsync("../Pages/_CardTypeAdd", ct);
				await _Hub.Clients.All.SendAsync("CardTypeListChanged", renderedViewStr);
				return new EmptyResult();
				
				//Response.ContentType = "text/vnd.turbo-stream.html";
				//return Partial("_CardTypeAdd", ct);
			}
			else
			{
				CardType ct = CardTypes.Instance.Where(x => x.Id == id).FirstOrDefault();
				ct.Definition = definition;
				ct.IsVisitable = isVisitable;
				ct.IsLocationRequired = isLocationRequired;
				ct.IsUserAccount = isUserAccount;

				var renderedViewStr = await _Renderer.RenderPartialToStringAsync("../Pages/_CardTypeEdit", ct);
				await _Hub.Clients.All.SendAsync("CardTypeListChanged", renderedViewStr);
				return new EmptyResult();

				//Response.ContentType = "text/vnd.turbo-stream.html";
				//return Partial("_CardTypeEdit", ct);
			}
		}
		else
		{
			if (check != null)
			{
				CardType ct = CardTypes.Instance.Where(x => x.Id == id).FirstOrDefault();
				if (ct is null)
				{
					ct = new CardType(0, string.Empty, false, false, false);
				}

				var renderedViewStr = await _Renderer.RenderPartialToStringAsync("../Pages/_DefinitionAlreadyExists", ct);
				await _Hub.Clients.All.SendAsync("CardTypeListChanged", renderedViewStr);
				return new EmptyResult();

				// return Partial("_DefinitionAlreadyExists", ct);
			}
			else
			{
				CardType ct = CardTypes.Instance.Where(x => x.Id == id).FirstOrDefault();
				if (ct is null)
				{
					ct = new CardType(0, string.Empty, false, false, false);
				}

				var renderedViewStr = await _Renderer.RenderPartialToStringAsync("../Pages/_DefinitionEmpty", ct);
				await _Hub.Clients.All.SendAsync("CardTypeListChanged", renderedViewStr);
				return new EmptyResult();

				// return Partial("_DefinitionEmpty", ct);
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

	public async Task<IActionResult> OnPostDelete(int id)
	{
		CardType ct = CardTypes.Instance.Where(x => x.Id == id).FirstOrDefault();
		CardTypes.Instance.Remove(ct);

		var renderedViewStr = await _Renderer.RenderPartialToStringAsync("../Pages/_CardTypeDelete", ct);
		await _Hub.Clients.All.SendAsync("CardTypeListChanged", renderedViewStr);
		return new EmptyResult();

		//Response.ContentType = "text/vnd.turbo-stream.html";
		//return Partial("_CardTypeDelete", ct);
	}
}