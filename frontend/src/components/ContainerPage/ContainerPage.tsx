export function ContainerPage({ children }: { children: JSX.Element | JSX.Element[] }) {
  return (
    <div className='container page'>
      <div className='row'> {children} </div>
    </div>
  );
}
