// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright © 2020-2022 Aman Agnihotri

namespace Telegram.Bots.Requests;

public sealed record Close : IRequest<bool>
{
  public string Method => "close";
}
