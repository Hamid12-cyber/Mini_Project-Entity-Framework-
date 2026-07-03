using Mini_Project_Entitiy_Framework_.Domain.Entities;
using Mini_Project_Entitiy_Framework_.Domain.Enums;
using Mini_Project_Entitiy_Framework_.Application.Exceptions;
using Mini_Project_Entitiy_Framework_.Application.Interfaces.Repositories;
using Mini_Project_Entitiy_Framework_.Application.Interfaces.Services;

namespace Mini_Project_Entitiy_Framework_.Application.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public Author CreateAuthor(string name, string? surname, Gender gender)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Author-un adı boş ola bilməz.");

        var author = new Author
        {
            Name = name.Trim(),
            Surname = string.IsNullOrWhiteSpace(surname) ? null : surname.Trim(),
            Gender = gender
        };

        _authorRepository.Add(author);
        _authorRepository.SaveChanges();
        return author;
    }

    public List<Author> GetAllAuthors()
    {
        return _authorRepository.GetAllWithBooks();
    }

    public Author? GetAuthorById(int id)
    {
        return _authorRepository.GetByIdWithBooks(id);
    }

    public List<Book> GetAuthorBooks(int authorId)
    {
        var author = _authorRepository.GetByIdWithBooks(authorId);
        if (author is null)
            throw new NotFoundException($"Id-si {authorId} olan Author tapılmadı.");

        return author.Books;
    }
}
