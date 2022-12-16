import { fireEvent, render, screen } from '@testing-library/react';
import { Pagination } from './Pagination';
import R from 'ramda';

it('Should render correct amount of page buttons', () => {
  const { rerender } = render(<Pagination currentPage={1} count={500} itemsPerPage={10} />);
  expectPageButtonCount(50);

  rerender(<Pagination currentPage={5} count={501} itemsPerPage={10} />);
  expectPageButtonCount(51);

  rerender(<Pagination currentPage={1} count={100} itemsPerPage={15} />);
  expectPageButtonCount(7);
});

function expectPageButtonCount(amount: number) {
  const buttons = screen.queryAllByLabelText(/Go to page number \d/);
  expect(buttons.length).toBe(amount);

  if (amount > 0) expect(parseInt(R.last(buttons)?.textContent || '0')).toBe(amount);
}

it("Should not render buttons if there's only 1 page", () => {
  render(<Pagination currentPage={1} count={10} itemsPerPage={10} />);
  expectPageButtonCount(0);
});

it('Should render active page as active and the rest as non active', () => {
  render(<Pagination currentPage={7} count={500} itemsPerPage={10} />);

  const buttons = screen.queryAllByLabelText(/Go to page number \d/);
  buttons.forEach((button, index) => {
    if (index === 7 - 1) {
      expect(button.parentElement).toHaveClass('active');
    } else {
      expect(button.parentElement).not.toHaveClass('active');
    }
  });
});

it('Should render active page as active and the rest as non active', () => {
  const mockOnPageChange = jest.fn();
  render(<Pagination currentPage={7} count={500} itemsPerPage={10} onPageChange={mockOnPageChange} />);

  fireEvent.click(screen.getByLabelText(/Go to page number 10/));
  fireEvent.click(screen.getByLabelText(/Go to page number 23/));

  expect(mockOnPageChange.mock.calls.length).toBe(2);
  expect(mockOnPageChange.mock.calls[0][0]).toBe(10);
  expect(mockOnPageChange.mock.calls[1][0]).toBe(23);
});
