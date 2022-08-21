﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdesseCardTypes.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		[BindProperty(SupportsGet = true)]
		public string UserName { get; set; }

		[BindProperty(SupportsGet = true)]
		public string Password { get; set; }

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public ActionResult OnPost()
		{

			if (string.IsNullOrEmpty(UserName))
			{
				return FormFail();
			}
			else
			{
				return FormSuccess();
			}
		}

		public RedirectToPageResult FormSuccess()
		{
			return new RedirectToPageResult("Menu", this);
		}

		public PartialViewResult FormFail()
		{
			// return new RedirectToPageResult("LoginError", this);
			return Partial("_LoginError", this);
		}
	}
}