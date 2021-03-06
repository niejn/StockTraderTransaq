//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using StockTraderTransaq.Infrastructure.Interfaces;
using StockTraderTransaq.Infrastructure.Models;
using StockTraderTransaq.Modules.Position.Properties;
using System.ComponentModel.Composition;

namespace StockTraderTransaq.Modules.Position.Services
{
    [Export(typeof(IAccountPositionService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class AccountPositionService : IAccountPositionService
    {
        List<AccountPosition> _positions = new List<AccountPosition>();

        public AccountPositionService()
        {
            InitializePositions();
        }

        #region IAccountPositionService Members

        public event EventHandler<AccountPositionModelEventArgs> Updated = delegate { };

        public IList<AccountPosition> GetAccountPositions()
        {
            return _positions;
        }
        #endregion

        private void InitializePositions()
        {
            using (var sr = new StringReader(Resources.AccountPositions))
            {
                XDocument document = XDocument.Load(sr);
                _positions = document.Descendants("AccountPosition")
                    .Select(
                    x => new AccountPosition(x.Element("TickerSymbol").Value,
                                             decimal.Parse(x.Element("CostBasis").Value, CultureInfo.InvariantCulture),
                                             long.Parse(x.Element("Shares").Value, CultureInfo.InvariantCulture)))
                    .ToList();
            }
        }

    }
}
