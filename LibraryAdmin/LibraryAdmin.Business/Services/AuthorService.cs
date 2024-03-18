using LibraryAdmin.Business.Mappers;
using LibraryAdmin.Business.Models;
using LibraryAdmin.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryAdmin.API.DtoModels;
using LibraryAdmin.API.ExceptionFilters;
using System.Threading;
using LibraryAdmin.DataAccess.Repositories.Contracts;

namespace LibraryAdmin.Business.Services
{
    public class AuthorService
    {
        IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository) 
        {
            _authorRepository = authorRepository;
        }
        public async Task<long?> CreateAuthor(CancellationToken cancellationToken, AuthorRequestModel authorDto)
        {
            try
            {
                var author = MapperRequestModels.MapToAuthor(authorDto);
                if (!Validator.IsValidAuthor(author, out var validationErrors))
                {
                    throw new InvalidAuthorException(validationErrors);
                }

                // Check cancellationToken before performing any asynchronous operation
                cancellationToken.ThrowIfCancellationRequested();

                var id = await _authorRepository.CreateAuthorEntity(cancellationToken, MapperDataAccess.MapToAuthorEntity(author));

                cancellationToken.ThrowIfCancellationRequested();

                return id;
            }
            catch (OperationCanceledException operationCancelled)
            {
                // Handle cancellation here if needed
                // You can log, clean up, or perform any other necessary actions
                // Re-throw the exception to propagate the cancellation further
                throw operationCancelled;
            }
            catch(InvalidAuthorException authorException)
            {
                throw authorException;
            }
            catch (Exception)
            {
                // Handle other exceptions here
                // You might want to log or handle specific exceptions differently
                //throw new MyCustomException("An error occurred while creating author", ex);
                throw;
            }
            
        }
        public async Task UpdateAuthor(CancellationToken cancellationToken, AuthorRequestModel authorDto)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var authorEntity = await _authorRepository.GetEntityById(cancellationToken, authorDto.Id, true);
                if (authorEntity == null)
                {
                    throw new AuthorNotFoundException(authorDto.Id);
                }
                bool isUpdate = false;
                if (!string.IsNullOrEmpty(authorDto.Name) && authorDto.Name != authorEntity.Name)
                {
                    authorEntity.Name = authorDto.Name;
                    isUpdate = true;
                }
                if (authorDto.BirthDate != null && authorDto.BirthDate != authorEntity.BirthDate)
                {
                    authorEntity.BirthDate = authorDto.BirthDate;
                    isUpdate = true;
                }
                if (!string.IsNullOrEmpty(authorDto.Genre) && authorDto.Genre != authorEntity.Genre)
                {
                    authorEntity.Genre = authorDto.Genre;
                    isUpdate = true;
                }

                if (isUpdate)
                {
                    var author = MapperDataAccess.MapToAuthor(authorEntity);
                    if (!Validator.IsValidAuthor(author, out var validationErrors))
                    {
                        throw new InvalidAuthorException(validationErrors);
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                    await _authorRepository.UpdateAuthorEntity(cancellationToken, authorEntity);

                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException operationCancelled)
            {
                throw operationCancelled;
            }
            catch (AuthorNotFoundException authorNotFoundException)
            {
                throw authorNotFoundException;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Author> GetById(CancellationToken cancellationToken, long? id)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var authorEntity = await _authorRepository.GetEntityById(cancellationToken, id, false);
                cancellationToken.ThrowIfCancellationRequested();

                if (authorEntity == null)
                {
                    throw new AuthorNotFoundException(id);
                }

                var author = MapperDataAccess.MapToAuthor(authorEntity);
                return author;
            }
            catch (OperationCanceledException operationCancelled)
            {
                throw operationCancelled;
            }
            catch (AuthorNotFoundException authorNotFoundException)
            {
                throw authorNotFoundException;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<List<Author>> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var authorEntities = await _authorRepository.GetAll(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();

                var authors = new List<Author>();

                foreach (var authorEntity in authorEntities)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var author = MapperDataAccess.MapToAuthor(authorEntity);
                    authors.Add(author);
                }

                return authors;
            }
            catch (OperationCanceledException operationCancelled)
            {
                throw operationCancelled;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
