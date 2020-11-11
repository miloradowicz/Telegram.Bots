// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

namespace Telegram.Bots.Requests
{
  public sealed record LogOut : IRequest<bool>
  {
    public string Method { get; } = "logOut";
  }
}
