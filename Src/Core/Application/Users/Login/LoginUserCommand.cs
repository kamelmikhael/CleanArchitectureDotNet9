﻿using Application.Abstractions.Messaging;

namespace Application.Users.Login;

public sealed record LoginUserCommand(string Username) : ICommand<string>;
