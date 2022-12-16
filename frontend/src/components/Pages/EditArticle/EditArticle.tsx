import React, { Fragment, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { getArticle, updateArticle } from '../../../services/conduit';
import { store } from '../../../state/store';
import { useStore } from '../../../state/storeHooks';
import { ArticleEditor } from '../../ArticleEditor/ArticleEditor';
import { initializeEditor, loadArticle, startSubmitting, updateErrors } from '../../ArticleEditor/ArticleEditor.slice';

export function EditArticle() {
  const { slug } = useParams<{ slug: string }>();
  const { loading } = useStore(({ editor }) => editor);

  useEffect(() => {
    _loadArticle(slug);
  }, [slug]);

  return <Fragment>{!loading && <ArticleEditor onSubmit={onSubmit(slug)} />}</Fragment>;
}

async function _loadArticle(slug: string) {
  store.dispatch(initializeEditor());
  try {
    const { title, description, body, tagList, author } = await getArticle(slug);

    if (author.username !== store.getState().app.user.unwrap().username) {
      location.hash = '#/';
      return;
    }

    store.dispatch(loadArticle({ title, description, body, tagList }));
  } catch {
    location.hash = '#/';
  }
}

function onSubmit(slug: string): (ev: React.FormEvent) => void {
  return async (ev) => {
    ev.preventDefault();

    store.dispatch(startSubmitting());
    const result = await updateArticle(slug, store.getState().editor.article);

    result.match({
      err: (errors) => store.dispatch(updateErrors(errors)),
      ok: ({ slug }) => {
        location.hash = `#/article/${slug}`;
      },
    });
  };
}
