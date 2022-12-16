import React from 'react';

export function FormGroup({
  type,
  placeholder,
  disabled,
  value,
  onChange,
  lg,
}: {
  type: string;
  placeholder: string;
  disabled: boolean;
  value: string;
  lg: boolean;
  onChange: (ev: React.ChangeEvent<HTMLInputElement>) => void;
}) {
  return (
    <fieldset className='form-group'>
      <input
        className={`form-control${!lg ? '' : ' form-control-lg'}`}
        {...{ type, placeholder, disabled, value, onChange }}
      />
    </fieldset>
  );
}

export function TextAreaFormGroup({
  type,
  placeholder,
  disabled,
  value,
  rows,
  onChange,
  lg,
}: {
  type: string;
  placeholder: string;
  disabled: boolean;
  rows: number;
  value: string;
  lg: boolean;
  onChange: (ev: React.ChangeEvent<HTMLTextAreaElement>) => void;
}) {
  return (
    <fieldset className='form-group'>
      <textarea
        className={`form-control${!lg ? '' : ' form-control-lg'}`}
        {...{ type, placeholder, disabled, value, onChange, rows }}
      ></textarea>
    </fieldset>
  );
}

export function ListFormGroup({
  type,
  placeholder,
  disabled,
  value,
  listValue,
  lg,
  onChange,
  onEnter,
  onRemoveItem,
}: {
  type: string;
  placeholder: string;
  disabled: boolean;
  value: string;
  listValue: string[];
  lg: boolean;
  onChange: (ev: React.ChangeEvent<HTMLInputElement>) => void;
  onEnter: () => void;
  onRemoveItem: (index: number) => void;
}) {
  return (
    <fieldset className='form-group'>
      <input
        className={`form-control${!lg ? '' : ' form-control-lg'}`}
        {...{ type, placeholder, disabled, value, onChange }}
        onKeyDown={(ev) => ev.key === 'Enter' && ev.preventDefault()}
        onKeyUp={onListFieldKeyUp(onEnter)}
      />
      <div className='tag-list'>
        {listValue.map((value, index) => (
          <span key={value} className='tag-default tag-pill' onClick={() => onRemoveItem(index)}>
            <i className='ion-close-round'></i>
            {value}
          </span>
        ))}
      </div>
    </fieldset>
  );
}

export function onListFieldKeyUp(onEnter: () => void): (ev: React.KeyboardEvent) => void {
  return (ev) => {
    if (ev.key === 'Enter') {
      ev.preventDefault();
      onEnter();
    }
  };
}
