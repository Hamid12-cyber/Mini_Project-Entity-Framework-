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

    public Author UpdateAuthor(int id, string name, string? surname, Gender gender)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Author-un adı boş ola bilməz.");

        var author = _authorRepository.GetById(id);
        if (author is null)
            throw new NotFoundException($"Id-si {id} olan Author tapılmadı.");

        author.Name = name.Trim();
        author.Surname = string.IsNullOrWhiteSpace(surname) ? null : surname.Trim();
        author.Gender = gender;

        _authorRepository.Update(author);
        _authorRepository.SaveChanges();
        return author;
    }

    public bool DeleteAuthor(int id)
    {
        var author = _authorRepository.GetByIdWithBooks(id);
        if (author is null)
            throw new NotFoundException($"Id-si {id} olan Author tapılmadı.");

        if (author.Books.Count > 0)
            throw new BusinessRuleException("Bu Author-un kitabları var, silinə bilməz. Əvvəlcə kitabları silin və ya başqa Author-a köçürün.");

        _authorRepository.Delete(author);
        _authorRepository.SaveChanges();
        return true;
    }

    public List<Author> SearchAuthors(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            throw new ValidationException("Axtarış açar sözü boş ola bilməz.");

        return _authorRepository.SearchByName(keyword.Trim());
    }
}
