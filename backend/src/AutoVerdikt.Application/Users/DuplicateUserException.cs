namespace AutoVerdikt.Application.Users;

public sealed class DuplicateUserException() : Exception("User is already registered.");
