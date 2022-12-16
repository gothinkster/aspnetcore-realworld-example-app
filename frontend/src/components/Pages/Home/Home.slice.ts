import { None, Option, Some } from '@hqoss/monads';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';

export interface HomeState {
  tags: Option<string[]>;
  selectedTab: string;
}

const initialState: HomeState = {
  tags: None,
  selectedTab: 'Global Feed',
};

const slice = createSlice({
  name: 'home',
  initialState,
  reducers: {
    startLoadingTags: () => initialState,
    loadTags: (state, { payload: tags }: PayloadAction<string[]>) => {
      state.tags = Some(tags);
    },
    changeTab: (state, { payload: tab }: PayloadAction<string>) => {
      state.selectedTab = tab;
    },
  },
});

export const { startLoadingTags, loadTags, changeTab } = slice.actions;

export default slice.reducer;
