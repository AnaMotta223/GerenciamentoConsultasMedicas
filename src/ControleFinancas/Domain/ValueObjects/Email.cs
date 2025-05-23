﻿using AppointmentsManager.Domain.Exceptions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AppointmentsManager.Domain.ValueObjects
{
    public class Email
    {
        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidEmailException("O email não pode ser vazio.");

            if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new InvalidEmailException("O email informado é inválido.");

            Value = value;
        }
        public override string ToString() => Value;
        public override bool Equals(object obj)
            => obj is Email other && Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();
    }
}
