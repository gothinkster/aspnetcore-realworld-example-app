import { range } from 'ramda';

export function Pagination({
  currentPage,
  count,
  itemsPerPage,
  onPageChange,
}: {
  currentPage: number;
  count: number;
  itemsPerPage: number;
  onPageChange?: (index: number) => void;
}) {
  return (
    <nav>
      <ul className='pagination'>
        {Math.ceil(count / itemsPerPage) > 1 &&
          range(1, Math.ceil(count / itemsPerPage) + 1).map((index) => (
            <li
              key={index}
              className={`page-item${currentPage !== index ? '' : ' active'}`}
              onClick={onPageChange && (() => onPageChange(index))}
            >
              <a className='page-link' aria-label={`Go to page number ${index}`} href='#'>
                {index}
              </a>
            </li>
          ))}
      </ul>
    </nav>
  );
}
