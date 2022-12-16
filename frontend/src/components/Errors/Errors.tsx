import { GenericErrors } from '../../types/error';

export function Errors({ errors }: { errors: GenericErrors }) {
  return (
    <ul className='error-messages'>
      {Object.entries(errors).map(([field, fieldErrors]) =>
        fieldErrors.map((fieldError) => (
          <li key={field + fieldError}>
            {field} {fieldError}
          </li>
        ))
      )}
    </ul>
  );
}
